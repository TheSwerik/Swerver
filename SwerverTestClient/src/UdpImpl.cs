using System;
using Swerver.Util;

namespace SwerverTestClient
{
    public class UdpImpl : Udp
    {
        public UdpImpl(string ip, int port) : base(ip, port) { }

        protected override void ExecuteOnMainThread(Action action) { }
    }
}