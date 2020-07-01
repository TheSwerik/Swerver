using System;
using System.Net.Sockets;

namespace ServerTest
{
    public class Tcp
    {
        private const int BufferSize = 4096;
        public TcpClient Socket;
        private int _id;
        private NetworkStream _stream;
        private byte[] _receiveBuffer;
        public Tcp(int id) { _id = id; }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Socket.SendBufferSize = BufferSize;

            _stream = Socket.GetStream();
            _receiveBuffer = new byte[BufferSize];

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
                Console.WriteLine($"Error receiving TCP Data: {e}");
                //TODO Disconnect
            }
        }
    }
}