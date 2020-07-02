using System;
using System.Net.Sockets;

namespace Swerver.Util
{
    public abstract class Tcp
    {
        private const int BufferSize = 4096;
        protected readonly int Id;
        private byte[] _receiveBuffer;
        private Packet _receivedData;
        private NetworkStream _stream;
        public TcpClient Socket;

        protected Tcp(int id) { Id = id; }
        protected Tcp() { }

        /// <summary>This should only be used by the Client!</summary>
        /// <param name="ip">Server IP</param>
        /// <param name="port">Server Port</param>
        public void Connect(string ip, int port)
        {
            Socket = new TcpClient
                     {
                         ReceiveBufferSize = BufferSize,
                         SendBufferSize = BufferSize
                     };

            _receiveBuffer = new byte[BufferSize];
            _receivedData = new Packet();

            Socket.BeginConnect(ip, port, ConnectCallback, Socket);
        }

        /// <summary>This should only be used by the Server!</summary>
        /// <param name="socket"></param>
        /// <param name="sendWelcome">The Send Method</param>
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
                Console.WriteLine($"Error receiving Server Data: {e}");
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
                ExecuteOnMainThread(packetBytes, Id);
                packetLength = 0;

                if (_receivedData.UnreadLength() < 4) continue;
                packetLength = _receivedData.ReadInt();
                if (packetLength <= 0) return true;
            }

            return packetLength <= 1;
        }

        protected abstract void ExecuteOnMainThread(Action action);

        private void ExecuteOnMainThread(byte[] packetBytes, int id)
        {
            ExecuteOnMainThread(() =>
                                {
                                    using var packet = new Packet(packetBytes);
                                    var packetId = packet.ReadInt();
                                    Client.Client.PacketHandlers[packetId](packet);
                                });
        }

        private delegate void PacketHandler(Packet packet);
    }
}