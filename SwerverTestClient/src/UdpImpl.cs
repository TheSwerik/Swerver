using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ServerLibrary.Client;

namespace SwerverTestClient
{
    public class UdpImpl : Udp
    {
        public UdpImpl(string ip, int port) : base(ip, port) { }

        protected override void ExecuteOnMainThread(Action action) { }
    }
}