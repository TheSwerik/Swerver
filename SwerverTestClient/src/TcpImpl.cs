using System;
using System.Windows;
using Swerver.Util;

namespace SwerverTestClient
{
    public class TcpImpl : Tcp
    {
        protected override void ExecuteOnMainThread(Action action) { Application.Current.Dispatcher.Invoke(action); }
    }
}