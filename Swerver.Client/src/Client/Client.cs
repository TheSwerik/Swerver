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

        public static void Init(Tcp tcp, Udp udp)
        {
            if (Instance == null)
            {
                Instance = new Client {Tcp = tcp, udp = udp};
                Instance.udp.Init(Instance.Ip, Port);
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