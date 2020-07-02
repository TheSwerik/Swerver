using System;
using System.Net;
using Swerver.Util;

namespace Swerver.Client
{
    internal static class ClientHandler
    {
        internal static void Welcome(Packet packet)
        {
            var msg = packet.ReadString();
            var id = packet.ReadInt();

            Console.WriteLine($"Message Received: {msg}");
            Client.Instance.Id = id;
            ClientSend.WelcomeReceived();

            Client.Instance.Udp.Connect(
                ((IPEndPoint) Client.Instance.Tcp.Socket.Client.LocalEndPoint).Port);
        }

        internal static void UdpTest(Packet packet)
        {
            var msg = packet.ReadString();

            Console.WriteLine($"Message Received: {msg}");
            ClientSend.UdpTestReceived();
        }
    }
}