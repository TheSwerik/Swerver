using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Swerver.Util;

namespace Swerver.Server
{
    public static class Server
    {
        public delegate void PacketHandler(int client, Packet packet);

        private static TcpListener _tcpListener;
        private static UdpClient _udpListener;

        public static Dictionary<int, PacketHandler> PacketHandlers;
        public static int MaxPlayers { get; private set; }
        private static int Port { get; set; }
        public static Dictionary<int, Client> Clients { get; private set; }

        internal static void Start(int maxPlayers, int port)
        {
            Console.WriteLine("Starting Server...");

            MaxPlayers = maxPlayers;
            Port = port;

            Clients = new Dictionary<int, Client>();
            InitializeServerData();

            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpCallback, null);

            _udpListener = new UdpClient(Port);
            _udpListener.BeginReceive(UdpReceiveCallback, null);

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

        private static void UdpReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _udpListener.EndReceive(result, ref clientEndPoint);
                _udpListener.BeginReceive(UdpReceiveCallback, null);

                if (data.Length < 4) return;

                using var packet = new Packet(data);
                var clientId = packet.ReadInt();

                if (clientId == 0) return;

                if (Clients[clientId].Udp.EndPoint == null)
                {
                    Clients[clientId].Udp.Connect(clientEndPoint);
                    return;
                }

                if (Clients[clientId].Udp.EndPoint.ToString() == clientEndPoint.ToString())
                    Clients[clientId].Udp.HandleData(packet);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving UDP data: {e}");
            }
        }

        public static void SendUdpData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if (clientEndPoint != null)
                    _udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to {clientEndPoint} via UDP: {e}");
            }
        }

        private static void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++) Clients.Add(i, new Client(i));

            PacketHandlers = new Dictionary<int, PacketHandler>
                             {
                                 {(int) ClientPackets.WelcomeReceived, ServerHandler.WelcomeReceived},
                                 {(int) ClientPackets.UdpTestReceived, ServerHandler.UdpTestReceived}
                             };
            Console.WriteLine("Initialized packets.");
        }
    }
}