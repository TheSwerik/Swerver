using System;
using System.Net;
using System.Net.Sockets;

namespace ServerTest
{
    public abstract class Udp
    {
        protected readonly int Id;
        public IPEndPoint EndPoint;
        public UdpClient Socket;

        public Udp(string ip, int port, int id)
        {
            Id = id;
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>Creates Connection.</summary>
        /// <param name="localPort">The Local Port, should be different from the Server Port.</param>
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
                packet.InsertInt(Id);
                Socket?.BeginSend(packet.ToArray(), packet.Length(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to Player {Id} via TCP: {e}");
            }
        }

        private bool HandleData(byte[] data)
        {
            using var packet = new Packet(data);

            var packetLength = packet.ReadInt();
            data = packet.ReadBytes(packetLength);

            ExecuteOnMainThread(data, 1);
            packetLength = 0;

            if (packet.UnreadLength() < 4) continue;
            packetLength = packet.ReadInt();
            if (packetLength <= 0) return true;

            return packetLength <= 1;
        }

        protected abstract void ExecuteOnMainThread(byte[] packetBytes, int id);
    }
}