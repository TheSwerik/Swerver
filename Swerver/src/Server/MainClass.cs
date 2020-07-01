namespace ServerLibrary.Server
{
    public static class MainClass
    {
        private static void Main(string[] args) { ServerStarter.Start(new GameLogic()); }
    }
}