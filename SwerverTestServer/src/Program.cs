using System;
using ServerLibrary.Server;

namespace SwerverTestServer
{
    internal static class Program
    {
        internal static void Main() { ServerStarter.Start(new EmptyLogic());}

        internal class EmptyLogic : GameLogic
        {
            protected override void Update(int delta) { Console.WriteLine(delta); }
        }
    }
}