using System;
using Swerver.Server;
using Swerver.Util;

namespace SwerverTestServer
{
    public static class SendAndHandel
    {
        public static void ReceiveLol(int client, Packet packet)
        {
            var id = packet.ReadInt();
            var msg = packet.ReadString();

            Console.WriteLine(
                $"{Server.Clients[client].Tcp.Socket.Client.RemoteEndPoint} is Player {client} and sent {msg}.");
            if (client != id) Console.WriteLine($"Player (ID: {client}) has assumed the wrong client ID ({id})!");

            SendLol(client);
        }

        public static void SendLol(int client)
        {
            using var packet = new Packet((int) PacketEnum.Lol);
            packet.Write("lol back");
            ServerSend.SendTcpData(packet);
        }
    }
}