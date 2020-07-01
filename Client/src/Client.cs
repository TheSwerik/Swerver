using System;
using System.Collections.Generic;
using System.Windows;

namespace ServerTest
{
    public class Client
    {
        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;

        private static Dictionary<int, PacketHandler> packetHandlers;
        public int Id = 0;
        public string Ip = "127.0.0.1";
        public Tcp tcp;

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

        private void Start() { tcp = new ClientTcp(); }

        public void ConnectToServer()
        {
            InitializeClientData();
            tcp.Connect(Ip, Port);
        }

        private static void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler> {{(int) ServerPackets.Welcome, ClientHandler.Welcome}};
            Console.WriteLine("Initialized packets.");
        }

        private delegate void PacketHandler(Packet packet);

        public class ClientTcp : Tcp
        {
            protected override void ExecuteOnMainThread(byte[] packetBytes, in int id)
            {
                Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          using var packet = new Packet(packetBytes);
                                                          var packetId = packet.ReadInt();
                                                          packetHandlers[packetId](packet);
                                                      });
            }
        }
    }
}