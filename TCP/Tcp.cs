using System;
using System.Net.Sockets;

namespace ServerTest
{
    public class Tcp
    {
        private const int BufferSize = 4096;
        private int _id;
        private byte[] _receiveBuffer;
        private NetworkStream _stream;
        public TcpClient Socket;
        public Tcp(int id) { _id = id; }
        public Tcp() { }

        /*
         * For Client
         */
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

        /*
         * For Client
         */
        private void ConnectCallback(IAsyncResult result)
        {
            Socket.EndConnect(result);

            if (!Socket.Connected) return;

            _stream = Socket.GetStream();

            _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
        }

        /*
         * For Server
         */
        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Socket.SendBufferSize = BufferSize;

            _stream = Socket.GetStream();
            _receiveBuffer = new byte[BufferSize];

            _stream.BeginRead(_receiveBuffer, 0, BufferSize, ReceiveCallback, null);
        }

        /*
         * For Server
         */
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
                Console.WriteLine($"Error receiving TCP Data: {e}");
                //TODO Disconnect
            }
        }
    }
}