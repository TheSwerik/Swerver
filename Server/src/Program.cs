using System;

namespace ServerTest
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Starting Server...");
            Server.Start(50, 46551);
            Console.ReadKey();
        }
    }
}