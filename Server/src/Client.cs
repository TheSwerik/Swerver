using System;
using System.Net.Sockets;

namespace ServerTest
{
    public class Client
    {
        private readonly int _id;
        public readonly Tcp tcp;

        public Client(int id)
        {
            _id = id;
            tcp = new Tcp(_id);
        }

        public class Tcp
        {
            private const int BufferSize = 4096;
            private readonly int _id;
            private byte[] _receiveBuffer;
            private Packet _receivedData;
            private NetworkStream _stream;
            public TcpClient Socket;
            public Tcp(int id) { _id = id; }

            private void ConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected) return;

                _stream = Socket.GetStream();

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

            public void Connect(TcpClient socket)
            {
                Socket = socket;
                Socket.ReceiveBufferSize = Socket.SendBufferSize = BufferSize;

                _stream = Socket.GetStream();
                _receiveBuffer = new byte[BufferSize];

                _receivedData = new Packet();

                _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);

                ServerSend.Welcome(_id, "Welcome to the Server.");
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
                    ThreadManager.ExecuteOnMainThread(() =>
                                                      {
                                                          using var packet = new Packet(packetBytes);
                                                          var packetId = packet.ReadInt();
                                                          Server.packetHandlers[packetId](_id, packet);
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