using Swerver.Server;
using Swerver.Util;

namespace SwerverTestServer
{
    internal static class Program
    {
        internal static void Main()
        {
            Constants.Port = 17777;
            Constants.MaxPlayers = 1;

            ServerStarter.Start(new EmptyLogic());
            Server.PacketHandlers.Add((int) PacketEnum.Lol, SendAndHandel.ReceiveLol);
        }

        private class EmptyLogic : GameLogic
        {
            protected override void Update(int delta) { }
        }
    }
}