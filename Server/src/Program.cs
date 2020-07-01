using System;

namespace ServerTest
{
    internal static class Program
    {
        private const int Port = 46551;

        internal static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Starting Server...");
            Server.Start(50, Port);
            Console.ReadKey();
        }
    }
}