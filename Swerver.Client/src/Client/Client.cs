using System;
using System.Collections.Generic;
using Swerver.Util;

namespace Swerver.Client
{
    public class Client
    {
        public delegate void PacketHandler(Packet packet);

        public static Client Instance;
        public static Dictionary<int, PacketHandler> PacketHandlers;
        public int Id;

        internal bool IsConnected;
        internal Tcp Tcp;
        internal Udp Udp;

        ~Client() { Disconnect(); }

        public static void Init(Tcp tcp, Udp udp)
        {
            if (Instance == null)
            {
                Instance = new Client {Tcp = tcp, Udp = udp};
                Instance.Udp.Init(Constants.Ip, Constants.Port);
            }
            else
            {
                Console.WriteLine("Instance already exists, destroying Object!");
            }
        }

        public void ConnectToServer()
        {
            InitializeClientData();
            IsConnected = true;
            Tcp.Connect();
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

        public void Disconnect()
        {
            if (!IsConnected) return;
            IsConnected = false;
            Tcp.Socket.Close();
            Udp.Socket.Close();

            Console.WriteLine("Disconnected from the Server.");
        }
    }
}