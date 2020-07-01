using System;
using System.Net;

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

            Client.Instance.Udp.Connect(((IPEndPoint) Client.Instance.Tcp.Socket.Client.LocalEndPoint).Port);
        }
    }
}