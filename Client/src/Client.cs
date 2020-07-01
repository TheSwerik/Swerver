using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;

namespace ServerTest
{
    public class Client
    {
        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;

        private static Dictionary<int, PacketHandler> packetHandlers;
        private Tcp _tcp;
        public int Id = 0;
        public string Ip = "127.0.0.1";

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

        private void Start() { _tcp = new Tcp(); }

        public void ConnectToServer()
        {
            InitializeClientData();
            _tcp.Connect(Ip, Port);
        }

        private static void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler> {{(int) ServerPackets.Welcome, ClientHandler.Welcome}};
            Console.WriteLine("Initialized packets.");
        }

        private delegate void PacketHandler(Packet packet);

        public class Tcp
        {
            private const int BufferSize = 4096;
            private readonly int _id;
            private byte[] _receiveBuffer;
            private NetworkStream _stream;
            private Packet receivedData;
            public TcpClient Socket;

            public void Connect(string ip, int port)
            {
                Socket = new TcpClient
                         {
                             ReceiveBufferSize = BufferSize,
                             SendBufferSize = BufferSize
                         };

                _receiveBuffer = new byte[BufferSize];
                Socket.BeginConnect(ip, port, ConnectCallback, Socket);
            }

            private void ConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected) return;

                _stream = Socket.GetStream();

                receivedData = new Packet();

                _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0) return; //TODO Disconnect

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);

                    receivedData.Reset(HandleData(data));

                    _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving ServerUtil Data: {e}");
                    //TODO Disconnect
                }
            }

            private bool HandleData(byte[] data)
            {
                var packetLength = 0;

                receivedData.SetBytes(data);

                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
                {
                    var packetBytes = receivedData.ReadBytes(packetLength);
                    Application.Current.Dispatcher.Invoke(() =>
                                                          {
                                                              using var packet = new Packet(packetBytes);
                                                              var packetId = packet.ReadInt();
                                                              packetHandlers[packetId](packet);
                                                          });
                    packetLength = 0;

                    if (receivedData.UnreadLength() < 4) continue;
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }
        }
    }
}