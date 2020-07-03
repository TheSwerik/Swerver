using System;
using System.Net.Sockets;

namespace Swerver.Util
{
    public abstract class Tcp
    {
        private const int BufferSize = 4096;
        private byte[] _receiveBuffer;
        private Packet _receivedData;
        private NetworkStream _stream;
        internal TcpClient Socket;

        internal void Connect()
        {
            Socket = new TcpClient
                     {
                         ReceiveBufferSize = BufferSize,
                         SendBufferSize = BufferSize
                     };

            _receiveBuffer = new byte[BufferSize];
            _receivedData = new Packet();

            Socket.BeginConnect(Constants.Ip, Constants.Port, ConnectCallback, Socket);
        }

        private void Disconnect()
        {
            Client.Client.Instance.Disconnect();
            _stream = null;
            _receivedData = null;
            _receiveBuffer = null;
            Socket = null;
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket.EndConnect(result);

            if (!Socket.Connected) return;

            _stream = Socket.GetStream();

            _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
        }

        internal void SendData(Packet packet)
        {
            try
            {
                if (Socket == null) return;
                _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to Player {Client.Client.Instance.Id} via TCP: {e}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (!Client.Client.Instance.IsConnected) return;
                var byteLength = _stream.EndRead(result);
                if (byteLength <= 0)
                {
                    Client.Client.Instance.Disconnect();
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
                Disconnect();
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

        protected abstract void ExecuteOnMainThread(Action action);

        private void ExecuteOnMainThread(byte[] packetBytes)
        {
            ExecuteOnMainThread(() =>
                                {
                                    using var packet = new Packet(packetBytes);
                                    var packetId = packet.ReadInt();
                                    Client.Client.PacketHandlers[packetId](packet);
                                });
        }
    }
}