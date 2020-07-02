using System;
using System.Net;
using ServerLibrary.Util;

namespace ServerLibrary.Client
{
    public class ClientHandler
    {
        public static void Welcome(Packet packet)
        {
            var msg = packet.ReadString();
            var id = packet.ReadInt();

            Console.WriteLine($"Message Received: {msg}");
            Swerver.Client.Client.Client.Instance.Id = id;
            ClientSend.WelcomeReceived();

            Swerver.Client.Client.Client.Instance.Udp.Connect(
                ((IPEndPoint) Swerver.Client.Client.Client.Instance.Tcp.Socket.Client.LocalEndPoint).Port);
        }

        public static void UdpTest(Packet packet)
        {
            var msg = packet.ReadString();

            Console.WriteLine($"Message Received: {msg}");
            ClientSend.UdpTestReceived();
        }
    }
}