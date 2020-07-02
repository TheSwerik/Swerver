using System;
using System.Threading;
using ServerLibrary.Util;

namespace ServerLibrary.Server
{
    public static class ServerStarter
    {
        /// <summary>The Port the Server will run on.</summary>
        public static int Port = 46551;

        /// <summary>The Maximum number of Clients that can connect to the Server.</summary>
        public static int MaxPlayers = 50;

        private static bool _isRunning;
        private static GameLogic _gameLogic;

        /// <summary>Starts the Server.</summary>
        /// <param name="gameLogic">The Instance of your GameLogic Implementation.</param>
        public static void Start(GameLogic gameLogic)
        {
            Console.Title = "Server";
            _isRunning = true;

            _gameLogic = gameLogic;

            var mainThread = new Thread(MainThread);
            mainThread.Start();
            Server.Start(MaxPlayers, Port);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TicksPerSec} ticks per second.");
            var nextLoop = DateTime.Now;
            var lastFrame = DateTime.Now;

            while (_isRunning)
            while (nextLoop < DateTime.Now)
            {
                _gameLogic.InternalUpdate((DateTime.Now - lastFrame).Milliseconds);
                lastFrame = DateTime.Now;
                
                nextLoop = nextLoop.AddMilliseconds(Constants.MsPerTick);

                if (nextLoop > DateTime.Now) Thread.Sleep(nextLoop - DateTime.Now);
            }
        }
    }
}