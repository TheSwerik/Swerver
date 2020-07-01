using System;
using System.Threading;

namespace ServerTest
{
    internal static class Program
    {
        private const int Port = 46551;
        private static bool _isRunning;

        internal static void Main(string[] args)
        {
            Console.Title = "Server";
            _isRunning = true;

            var mainThread = new Thread(MainThread);
            mainThread.Start();
            Server.Start(50, Port);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TicksPerSec} ticks per second.");
            var nextLoop = DateTime.Now;

            while (_isRunning)
            while (nextLoop < DateTime.Now)
            {
                GameLogic.Update();

                nextLoop = nextLoop.AddMilliseconds(Constants.MsPerTick);

                if (nextLoop > DateTime.Now) Thread.Sleep(nextLoop - DateTime.Now);
            }
        }
    }
}