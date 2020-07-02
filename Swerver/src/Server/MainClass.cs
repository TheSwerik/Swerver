namespace ServerLibrary.Server
{
    internal static class MainClass
    {
        internal static void Main(string[] args) { ServerStarter.Start(new GameLogic()); }
    }
}