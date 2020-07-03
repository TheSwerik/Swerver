using System;
using Swerver.Util;

namespace Swerver.Server
{
    public class Client
    {
        public readonly int Id;
        public readonly Tcp Tcp;
        internal readonly Udp Udp;

        internal Client(int id)
        {
            Id = id;
            Tcp = new Tcp(Id);
            Udp = new Udp(Id);
        }

        public void Disconnect()
        {
            Console.WriteLine($"{Tcp.Socket.Client.RemoteEndPoint} has disconnected.");

            Tcp.Disconnect();
            Udp.Disconnect();
        }
    }
}