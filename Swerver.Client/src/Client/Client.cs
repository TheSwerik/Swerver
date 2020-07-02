using System;
using System.Collections.Generic;
using ServerLibrary.Client;
using ServerLibrary.Util;

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
        public string Ip = "127.0.0.1";
        public Tcp Tcp;
        public Udp udp;

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new Client();
                Instance.Start();
            }
            else
            {
                Console.WriteLine("Instance already exists, destroying Object!");
            }
        }

        private void Start()
        {
            Tcp = new ClientTcp();
            udp = new Udp(Ip, Port);
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