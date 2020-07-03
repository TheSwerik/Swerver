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

        private bool _isConnected;
        public int Id;
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
            _isConnected = true;
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
            if (!_isConnected) return;
            _isConnected = false;
            Tcp.Socket.Close();
            Udp.Socket.Close();

            Console.WriteLine("Disconnected from the Server.");
        }
    }
}