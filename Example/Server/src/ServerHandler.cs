using System;

namespace ServerTest
{
    public class ServerHandler
    {
        public static void WelcomeReceived(int client, Packet packet)
        {
            var id = packet.ReadInt();
            var username = packet.ReadString();

            Console.WriteLine(
                $"{Server.Clients[client].Tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {client}.");
            if (client != id)
                Console.WriteLine($"Player \"{username}\" (ID: {client}) has assumed the wrong client ID ({id})!");

            // TODO: send player into game
        }

        public static void UdpTestReceived(int client, Packet packet)
        {
            var msg = packet.ReadString();

            Console.WriteLine($"Received a Message via UDP. Message: {msg}.");
        }
    }
}