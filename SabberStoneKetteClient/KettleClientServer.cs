﻿using SabberStoneKettle;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SabberStoneKettleClient
{
    class KettleClientServer
    {
        private Socket Listener;

        public KettleClientServer(IPEndPoint address)
        {

            Listener = new Socket(SocketType.Stream, ProtocolType.IP);
            Listener.Bind(address);
            Listener.Listen(1);
            Console.WriteLine("Listening on socket.");
        }
        
        public void OnStartClient(KettleStartClient StartClient)
        {
            // and then we start a new session in a new thread
            var session = new KettleAISession(StartClient);
            new Thread(session.Enter).Start();
        }
        
        public void Enter()
        {
            // We wait until someone connects to this server
            Socket gameserver = Listener.Accept();
            KettleAdapter gameadapter = new KettleAdapter(new NetworkStream(gameserver));
            gameadapter.OnStartClient += OnStartClient;

            while (true)
            {
                try
                {
                    if (!gameadapter.HandleNextPacket())
                    {
                        Console.WriteLine("Kettle AI server ended.");
                        return;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Connection closed");
                    return;
                }
            }
        }
    }
}
