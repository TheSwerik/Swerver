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

        private void Start() { tcp = new Tcp(); }

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

        public class Tcp
        {
            private const int BufferSize = 4096;
            private readonly int _id;
            private byte[] _receiveBuffer;
            private Packet _receivedData;
            private NetworkStream _stream;
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

                _receivedData = new Packet();

                _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (Socket == null) return;
                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data to Player {_id} via TCP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0) return; //TODO Disconnect

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);

                    _receivedData.Reset(HandleData(data));

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

                _receivedData.SetBytes(data);

                if (_receivedData.UnreadLength() >= 4)
                {
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                while (packetLength > 0 && packetLength <= _receivedData.UnreadLength())
                {
                    var packetBytes = _receivedData.ReadBytes(packetLength);
                    Application.Current.Dispatcher.Invoke(() =>
                                                          {
                                                              using var packet = new Packet(packetBytes);
                                                              var packetId = packet.ReadInt();
                                                              packetHandlers[packetId](packet);
                                                          });
                    packetLength = 0;

                    if (_receivedData.UnreadLength() < 4) continue;
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }
        }
    }
}