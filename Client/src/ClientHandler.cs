using System;

namespace ServerTest
{
    public class ClientHandler
    {
        public static void Welcome(Packet packet)
        {
            var msg = packet.ReadString();
            var id = packet.ReadInt();

            Console.WriteLine($"Message Received: {msg}");
            Client.Instance.Id = id;
            ClientSend.WelcomeReceived();
        }
    }
}