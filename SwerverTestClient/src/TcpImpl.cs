using System;
using ServerLibrary.Client;

namespace SwerverTestClient
{
    public class TcpImpl : Client.ClientTcp
    {
        public TcpImpl(string ip, int port) : base(ip, port) { }

        protected override void ExecuteOnMainThread(Action action) { throw new NotImplementedException(); }
    }
}