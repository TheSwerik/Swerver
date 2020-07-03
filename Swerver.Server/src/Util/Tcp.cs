using System;
using System.Net.Sockets;

namespace Swerver.Util
{
    public class Tcp
    {
        private const int BufferSize = 4096;
        protected readonly int Id;
        private byte[] _receiveBuffer;
        private Packet _receivedData;
        private NetworkStream _stream;
        public TcpClient Socket;

        internal Tcp(int id) { Id = id; }

        public void Connect(TcpClient socket, Action<int, string> sendWelcome)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Socket.SendBufferSize = BufferSize;

            _stream = Socket.GetStream();

            _receiveBuffer = new byte[BufferSize];
            _receivedData = new Packet();


            _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);

            sendWelcome(Id, "Welcome to the Server.");
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket.EndConnect(result);

            if (!Socket.Connected) return;

            _stream = Socket.GetStream();

            _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
        }

        /// <summary></summary>
        /// <param name="packet"></param>
        public void SendData(Packet packet)
        {
            try
            {
                if (Socket == null) return;
                _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to Player {Id} via TCP: {e}");
            }
        }

        internal void Disconnect()
        {
            Socket.Close();
            _stream = null;
            _receivedData = null;
            _receiveBuffer = null;
            Socket = null;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var byteLength = _stream.EndRead(result);
                if (byteLength <= 0)
                {
                    Server.Server.Clients[Id].Disconnect();
                    return;
                }

                var data = new byte[byteLength];
                Array.Copy(_receiveBuffer, data, byteLength);
                _receivedData.Reset(HandleData(data));
                _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving Server Data: {e}");
                Server.Server.Clients[Id].Disconnect();
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
                ExecuteOnMainThread(packetBytes);
                packetLength = 0;

                if (_receivedData.UnreadLength() < 4) continue;
                packetLength = _receivedData.ReadInt();
                if (packetLength <= 0) return true;
            }

            return packetLength <= 1;
        }

        private void ExecuteOnMainThread(byte[] packetBytes)
        {
            ThreadManager.ExecuteOnMainThread(() =>
                                              {
                                                  using var packet = new Packet(packetBytes);
                                                  var packetId = packet.ReadInt();
                                                  Server.Server.PacketHandlers[packetId](Id, packet);
                                              });
        }

        private delegate void PacketHandler(Packet packet);
    }
}