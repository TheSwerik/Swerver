using System;
using System.Net.Sockets;

namespace ServerTest
{
    public class Client
    {
        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;
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

        public void ConnectToServer() { _tcp.Connect(Ip, Port); }

        public class Tcp
        {
            private const int BufferSize = 4096;
            private readonly int _id;
            private byte[] _receiveBuffer;
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
                    //TODO Handle Data
                    _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving ServerUtil Data: {e}");
                    //TODO Disconnect
                }
            }
        }
    }
}