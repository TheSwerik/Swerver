using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    public static class Server
    {
        public delegate void PacketHandler(int client, Packet packet);

        private static TcpListener _tcpListener;

        public static Dictionary<int, PacketHandler> packetHandlers;
        public static int MaxPlayers { get; set; }
        private static int Port { get; set; }
        public static Dictionary<int, Client> Clients { get; set; }

        public static void Start(int maxPlayers, int port)
        {
            Console.WriteLine("Starting Server...");

            MaxPlayers = maxPlayers;
            Port = port;

            Clients = new Dictionary<int, Client>();
            InitializeServerData();

            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpCallback, null);

            Console.WriteLine($"Server Started on {Port}.");
        }

        private static void TcpCallback(IAsyncResult result)
        {
            var client = _tcpListener.EndAcceptTcpClient(result);
            _tcpListener.BeginAcceptTcpClient(TcpCallback, null);
            Console.WriteLine($"Incoming connection for {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].Tcp.Socket != null) continue;
                Clients[i].Tcp.Connect(client, ServerSend.Welcome);
                return;
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++) Clients.Add(i, new Client(i));

            packetHandlers = new Dictionary<int, PacketHandler>
                             {{(int) ClientPackets.WelcomeReceived, ServerHandler.WelcomeReceived}};
            Console.WriteLine("Initialized packets.");
        }

        public static void SendUDPData(IPEndPoint endPoint, Packet packet) { throw new NotImplementedException(); }
    }
}