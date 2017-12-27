﻿using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using System;
using System.Collections.Generic;

namespace SabberStoneCore.Config
{
	/// <summary>
	/// Holds all configuration values to create a new <see cref="Game"/> instance.
	/// </summary>
	public class GameConfig
	{
		/// <summary>
		/// The default value for <see cref="StartPlayer"/>.
		/// -1 means the game will randomly pick one player as starting player.
		/// </summary>
		public const int START_PLAYER_DEFAULT = -1;

		/// <summary>
		/// The default name for players. The token will be replaced
		/// by the player index.
		/// </summary>
		public const string PLAYER_NAME_DEFAULT = "Player{0}";

		/// <summary>
		/// Gets or sets the index of the starting player.
		/// This value is 1-indexed, where 1 indicates <see cref="Player1Name"/> will start.
		/// </summary>
		/// <value>The starting player index.</value>
		public int StartPlayer { get; set; } = START_PLAYER_DEFAULT;

		/// <summary>
		/// Gets or sets the name of the player with index 1.
		/// </summary>
		/// <value>The name of the player.</value>
		public string Player1Name { get; set; } = String.Format(PLAYER_NAME_DEFAULT, 1);

		/// <summary>
		/// Gets or sets the name of the player with index 2.
		/// </summary>
		/// <value>The name of the player.</value>
		public string Player2Name { get; set; } = String.Format(PLAYER_NAME_DEFAULT, 2);

		/// <summary>
		/// Gets or sets the card class of the player with index 1.
		/// </summary>
		/// <value><see cref="CardClass"/></value>
		public CardClass Player1HeroClass { get; set; } = CardClass.HUNTER;

		/// <summary>
		/// Gets or sets the card class of the player with index 2.
		/// </summary>
		/// <value><see cref="CardClass"/></value>
		public CardClass Player2HeroClass { get; set; } = CardClass.MAGE;

		/// <summary>
		/// Gets or sets the card class of the player with index 1.
		/// </summary>
		/// <value><see cref="CardClass"/></value>
		public Card Player1HeroCard { get; set; }

		/// <summary>
		/// Gets or sets the card class of the player with index 2.
		/// </summary>
		/// <value><see cref="CardClass"/></value>
		public Card Player2HeroCard { get; set; }

		/// <summary>
		/// Gets or sets the format of the game.
		/// This value influences the game rules, see <see cref="FormatType"/>
		/// for more information.
		/// </summary>
		/// <value><see cref="FormatType"/></value>
		public FormatType FormatType { get; set; } = FormatType.FT_STANDARD;

		/// <summary>
		/// Gets or sets the deck of the player with index 1.</summary>
		/// <value><see cref="List{Card}"/></value>
		public List<Card> Player1Deck { get; set; } = null;

		/// <summary>
		/// Gets or sets the deck of the player with index 2.</summary>
		/// <value><see cref="List{Card}"/></value>
		/// <autogeneratedoc />
		public List<Card> Player2Deck { get; set; } = null;

		/// <summary>
		/// Gets or sets a value indicating whether the game should autofill the
		/// decks of all players.
		/// This option can be used in conjunction with <see cref="Player1Deck"/> and <see cref="Player2Deck"/>
		/// since it will only add cards until the deck limit is reached.
		/// </summary>
		/// <value><c>true</c> if decks need to be filled; otherwise, <c>false</c>.</value>
		public bool FillDecks { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the game should PREDICTABLY autofill the
		/// decks of all players. Set this property to true for predictable test cases!
		/// This option is only applicable when <see cref="FillDecks"/> is set to true.
		/// </summary>
		/// <value><c>true</c> if decks need to be filled PREDICTABLY; otherwise, <c>false</c>.</value>
		public bool FillDecksPredictably {get; set; } = false;

		/// <summary>
		/// List of <see cref="Card.Id"/>s which result in unpredictable test outcomes when included
		/// in a <see cref="Controller"/>'s deck.
		/// </summary>
		public List<string> UnPredictableCardIDs = new List<string> {
			"KAR_096", // Prince Malchezaar
			"CFM_637", // Patches the Pirate

			"UNG_028", // Quests
			"UNG_067",
			"UNG_116",
			"UNG_829",
			"UNG_934",
			"UNG_920",
			"UNG_940",
			"UNG_942",
			"UNG_954",

			"LOOT_149", // Corridor Creeper (trigger on board)
			"CFM_064", // Blubber Baron (trigger on board)
		};

		/// <summary>
		/// Unimplemented feature, the intention was to have all possible allowed cards for a draw in
		/// a pool from which it draws.
		/// </summary>
		public bool DrawPool { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Game"/> should shuffle
		/// all decks before starting.</summary>
		/// <value><c>true</c> if shuffling is needed; otherwise, <c>false</c>.</value>
		public bool Shuffle { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="Game"/> must split itself
		/// when random events occur. Enabling this feature will reduce performance! At the moment
		/// this feature is only prototyped with the mad bomber tests.
		/// </summary>
		/// <value><c>true</c> if splitting is necessary; otherwise, <c>false</c>.</value>
		public bool Splitting { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="Game"/> should store log entries.</summary>
		/// <value><c>true</c> if logging is required; otherwise, <c>false</c>.</value>
		public bool Logging { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="Game"/> should store POWER history
		/// entries.
		/// </summary>
		/// <value><c>true</c> if POWER history building is required; otherwise, <c>false</c>.</value>
		public bool History { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="Game"/> should skip the Mulligan phase.
		/// </summary>
		/// <value><c>true</c> if Mulligan must be skipped; otherwise, <c>false</c>.</value>
		public bool SkipMulligan { get; set; } = true;

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		public GameConfig Clone()
		{
			return (GameConfig)MemberwiseClone();
		}

	}
}
