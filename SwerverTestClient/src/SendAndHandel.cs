using System;
using Swerver.Client;
using Swerver.Util;

namespace SwerverTestClient
{
    public static class SendAndHandel
    {
        public static void ReceiveLol(Packet packet)
        {
            var msg = packet.ReadString();

            Console.WriteLine($"Message Received: {msg}");
            SendLol();
        }

        public static void SendLol()
        {
            using var packet = new Packet((int) PacketEnum.Lol);
            packet.Write(Client.Instance.Id);
            packet.Write("lol");

            ClientSend.SendTcpData(packet);
        }
    }
}