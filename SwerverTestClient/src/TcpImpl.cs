using System;
using ServerLibrary.Util;

namespace SwerverTestClient
{
    public class TcpImpl : Tcp
    {
        public TcpImpl(string ip, int port) : base(ip, port) { }

        protected override void ExecuteOnMainThread(Action action) { throw new NotImplementedException(); }
        protected override void ExecuteOnMainThread(byte[] packetBytes, int id) { throw new NotImplementedException(); }
    }
}