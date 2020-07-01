using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ServerLibrary.Util;

namespace ServerLibrary.Client
{
    public class Client
    {
        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;

        private static Dictionary<int, PacketHandler> packetHandlers;
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
            packetHandlers = new Dictionary<int, PacketHandler>
                             {
                                 {(int) ServerPackets.Welcome, ClientHandler.Welcome},
                                 {(int) ServerPackets.UdpTest, ClientHandler.UdpTest}
                             };
            Console.WriteLine("Initialized packets.");
        }

        private delegate void PacketHandler(Packet packet);

        public class ClientTcp : Tcp
        {
            protected override void ExecuteOnMainThread(byte[] packetBytes, int id)
            {
                Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          using var packet = new Packet(packetBytes);
                                                          var packetId = packet.ReadInt();
                                                          packetHandlers[packetId](packet);
                                                      });
            }
        }

        public class Udp
        {
            public IPEndPoint EndPoint;
            public UdpClient Socket;

            public Udp(string ip, int port) { EndPoint = new IPEndPoint(IPAddress.Parse(ip), port); }

            public void Connect(int localPort)
            {
                Socket = new UdpClient(localPort);

                Socket.Connect(EndPoint);

                Socket.BeginReceive(ReceiveCallback, null);

                using var packet = new Packet();
                SendData(packet);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var data = Socket.EndReceive(result, ref EndPoint);
                    Socket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4)
                        //TODO Disconnect
                        return;

                    HandleData(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving Server Data: {e}");
                    //TODO Disconnect
                }
            }

            public void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(Instance.Id);
                    Socket?.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data to Player {Instance.Id} via Udp: {e}");
                }
            }

            private void HandleData(byte[] data)
            {
                using var packet = new Packet(data);

                var packetLength = packet.ReadInt();
                data = packet.ReadBytes(packetLength);

                Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          using var packet = new Packet(data);
                                                          var packetId = packet.ReadInt();
                                                          packetHandlers[packetId](packet);
                                                      });
            }
        }
    }
}