using System;
using System.Net.Sockets;

namespace ServerTest
{
    public class Client
    {
        private readonly int _id;
        public readonly Tcp Tcp;

        public Client(int id)
        {
            _id = id;
            Tcp = new Tcp(_id);
        }

    }
}