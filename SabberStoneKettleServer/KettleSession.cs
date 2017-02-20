﻿using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Kettle;
using Newtonsoft.Json.Linq;

namespace SabberStoneKettleServer
{
    class KettleSession
    {
        private Socket Client;
        private KettleAdapter Adapter;
        private Game _game;

        public KettleSession(Socket client)
        {
            Client = client;
            Adapter = new KettleAdapter(new NetworkStream(client));
            Adapter.OnCreateGame += OnCreateGame;
            Adapter.OnConcede += OnConcede;
            Adapter.OnChooseEntities += OnChooseEntities;
            Adapter.OnSendOption += OnSendOption;
        }

        public void Enter()
        {
            while (true)
            {
                Console.WriteLine("Handling next packet..");
                if (!Adapter.HandleNextPacket())
                {
                    Console.WriteLine("Kettle session ended.");
                    return;
                }
                Console.WriteLine("Done!");
            }
        }

        public void OnConcede(int entityid)
        {
            Console.WriteLine($"player[{entityid}] concedes");
            var concedingPlayer = _game.ControllerById(entityid);
            _game.Process(ConcedeTask.Any(concedingPlayer));
            SendPowerHistory(_game.PowerHistory.Last);
        }

        public void OnSendOption(KettleSendOption sendOption)
        {

        }

        public void OnChooseEntities(List<int> chooseEntities)
        {

        }

        public void OnCreateGame(KettleCreateGame createGame)
        {
            Console.WriteLine("creating game");
            _game = new Game(new GameConfig
                    {
                        StartPlayer = 1,
                        Player1HeroClass = CardClass.PRIEST,
                        Player2HeroClass = CardClass.HUNTER,
                        SkipMulligan = false,
                        FillDecks = true
                    });

            // Start the game and send the following powerhistory to the client
            _game.StartGame();
            SendPowerHistory(_game.PowerHistory.Last);

            // getting choices mulligan choices for players ...
            var entityChoicesPlayer1 = PowerChoicesBuilder.EntityChoices(_game.Player1.Choice);
            var entityChoicesPlayer2 = PowerChoicesBuilder.EntityChoices(_game.Player2.Choice);

            // getting options for currentPlayer ...
            var options = PowerOptionsBuilder.AllOptions(_game.CurrentPlayer.Options());
        }

        private Dictionary<int, int> GameTagsToKettleTags(Dictionary<GameTag, int> tags)
        {
            var ktags = new Dictionary<int, int>();

            foreach (var tag in tags)
                ktags.Add((int)tag.Key, tag.Value);

            return ktags;
        }

        private KettleEntity GameEntityToKettleEntity(PowerEntity entity)
        {
            return new KettleEntity
            {
                EntityID = entity.Id,
                Tags = GameTagsToKettleTags(entity.Tags),
            };
        }

        private KettleEntity GameEntityToKettleEntity(PowerHistoryEntity entity)
        {
            return new KettleEntity
            {
                EntityID = entity.Id,
                Tags = GameTagsToKettleTags(entity.Tags),
            };
        }

        private KettleSubOption GameSubOptionToKettleSubOption(PowerSubOption option)
        {
            return new KettleSubOption
            {
                ID = option.EntityId,
                Targets = option.Targets,
            };
        }

        private void SendOptions(PowerAllOptions options)
        {
            KettleOptionsBlock block = new KettleOptionsBlock();
            block.ID = options.Index;
            block.Options = new List<KettleOption>();

            foreach (var option in options.PowerOptionList)
            {
                var koption = new KettleOption
                {
                    Type = (int)option.OptionType,
                    MainOption = GameSubOptionToKettleSubOption(option.MainOption),
                    SubOptions = new List<KettleSubOption>(),
                };

                foreach (var sub in option.SubOptions)
                    koption.SubOptions.Add(GameSubOptionToKettleSubOption(sub));

                block.Options.Add(koption);
            }

            Adapter.SendMessage(Adapter.CreatePayload(block));
        }

        private void SendPowerHistory(List<IPowerHistoryEntry> history)
        {
            List<JObject> message = new List<JObject>();
            foreach (var entry in history)
            {
                switch (entry.PowerType)
                {
                    case PowerType.CREATE_GAME:
                        message.Add(Adapter.CreatePayload(CreatePowerHistoryCreateGame((PowerHistoryCreateGame)entry)));
                        break;
                    case PowerType.FULL_ENTITY:
                        message.Add(Adapter.CreatePayload(CreatePowerHistoryFullEntity((PowerHistoryFullEntity)entry)));
                        break;
                    //case PowerType.SHOW_ENTITY:
                    //    message.Add(Adapter.CreatePayload(CreatePowerHistoryShowEntity((PowerHistoryShowEntity)entry)));
                    //    break;
                    case PowerType.TAG_CHANGE:
                        message.Add(Adapter.CreatePayload(CreatePowerHistoryTagChange((PowerHistoryTagChange)entry)));
                        break;
                    /*case PowerType.HIDE_ENTITY:
                        SendPowerHistoryChangeEntity((SendPowerHistoryChangeEntity)entry);
                        break;*/
                    default:
                        Console.WriteLine("Error, unhandled powertype " + entry.PowerType.ToString());
                        break;
                }
            }
            Adapter.SendMessage(message);
        }
        private KettleHistoryCreateGame CreatePowerHistoryCreateGame(PowerHistoryCreateGame p)
        {
            p.Game.Tags[GameTag.ZONE] = (int)Zone.PLAY;
            p.Game.Tags[GameTag.ENTITY_ID] = 1;
            p.Game.Tags[GameTag.CARDTYPE] = (int)CardType.GAME;

            var k = new KettleHistoryCreateGame
            {
                Game = new KettleEntity
                {
                    EntityID = p.Game.Id,
                    Tags = GameTagsToKettleTags(p.Game.Tags),
                },
                Players = new List<KettlePlayer>(),
            };

            Console.WriteLine("game tags are:");
            foreach (var tag in k.Game.Tags)
                Console.WriteLine("key: " + tag.Key + ", value: " + tag.Value);

            foreach (var player in p.Players)
            {
                k.Players.Add(new KettlePlayer
                {
                    Entity = GameEntityToKettleEntity(player.PowerEntity),
                    PlayerID = player.PlayerId,
                    CardBack = player.CardBack,
                });
            }

            Console.WriteLine("card back is :" + k.Players[0].CardBack);
            Console.WriteLine("card back is :" + k.Players[1].CardBack);

            return k;
        }

        private KettleHistoryFullEntity CreatePowerHistoryFullEntity(PowerHistoryFullEntity p)
        {
            return new KettleHistoryFullEntity
            {
                Name = p.Entity.Name,
                Entity = GameEntityToKettleEntity(p.Entity),
            };
        }

        private KettleHistoryShowEntity SendPowerHistoryShowEntity(PowerHistoryShowEntity p)
        {
            return new KettleHistoryShowEntity
            {
                Name = p.Entity.Name,
                Entity = GameEntityToKettleEntity(p.Entity),
            };
        }

        private KettleHistoryTagChange CreatePowerHistoryTagChange(PowerHistoryTagChange p)
        {
            return new KettleHistoryTagChange
            {
                EntityID = p.EntityId,
                Tag = (int)p.Tag,
                Value = p.Value,
            };
        }

    }
}