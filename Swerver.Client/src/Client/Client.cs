using System;
using System.Collections.Generic;
using Swerver.Util;

namespace Swerver.Client
{
    public class Client
    {
        public delegate void PacketHandler(Packet packet);

        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;

        public static Dictionary<int, PacketHandler> PacketHandlers;
        public int Id;
        public string Ip;
        public Tcp Tcp;
        public Udp Udp;
        public string Username;

        public static void Init(Tcp tcp, Udp udp, string ip = "127.0.0.1")
        {
            if (Instance == null)
            {
                Instance = new Client {Tcp = tcp, Udp = udp, Ip = ip};
                Instance.Udp.Init(Instance.Ip, Port);
            }
            else
            {
                Console.WriteLine("Instance already exists, destroying Object!");
            }
        }

        public void ConnectToServer()
        {
            InitializeClientData();
            Tcp.Connect(Ip, Port);
        }

        private static void InitializeClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>
                             {
                                 {(int) ServerPackets.Welcome, ClientHandler.Welcome},
                                 {(int) ServerPackets.UdpTest, ClientHandler.UdpTest}
                             };
            Console.WriteLine("Initialized packets.");
        }
    }
}