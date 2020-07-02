using System;
using System.Threading;
using Swerver.Util;

namespace Swerver.Server
{
    public static class ServerStarter
    {
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
            Server.Start(Constants.MaxPlayers, Constants.Port);
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