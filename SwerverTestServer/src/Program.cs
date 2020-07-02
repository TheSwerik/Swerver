using Swerver.Server;

namespace SwerverTestServer
{
    internal static class Program
    {
        internal static void Main() { ServerStarter.Start(new EmptyLogic()); }

        private class EmptyLogic : GameLogic
        {
            protected override void Update(int delta) { }
        }
    }
}