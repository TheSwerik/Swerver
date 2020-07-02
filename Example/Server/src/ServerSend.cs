using System.Linq;

namespace ServerTest
{
    public class ServerSend
    {
        /// <summary>Send TCP Data to a Client.</summary>
        /// <param name="client">ID of the Client</param>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(int client, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[client].Tcp.SendData(packet);
        }

        /// <summary>Send UDP Data to a Client.</summary>
        /// <param name="client">ID of the Client</param>
        /// <param name="packet">Data to be sent</param>
        private static void SendUdpData(int client, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[client].udp.SendData(packet);
        }

        /// <summary>Send TCP Data to all Clients.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.Clients[i].Tcp.SendData(packet);
        }

        /// <summary> Send TCP Data to all Clients except one.</summary>
        /// <param name="packet">Data to be sent</param>
        /// <param name="except">ID of the Client that does not receive the Data</param>
        private static void SendTcpData(Packet packet, int except)
        {
            packet.WriteLength();
            for (var i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i == except) continue;
                foreach (var client in Server.Clients
                                             .Where(c => c.Key != except)
                                             .Select(a => a.Value)) client.Tcp.SendData(packet);
            }
        }

        /// <summary>Send UDP Data to all Clients.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.Clients[i].udp.SendData(packet);
        }

        /// <summary>Send UDP Data to all Clients except one.</summary>
        /// <param name="packet">Data to be sent</param>
        /// <param name="except">ID of the Client that does not receive the Data</param>
        private static void SendUdpData(Packet packet, int except)
        {
            packet.WriteLength();
            foreach (var client in Server.Clients
                                         .Where(c => c.Key != except)
                                         .Select(a => a.Value)) client.udp.SendData(packet);
        }

        #region Packets

        /// <summary>Sends a welcome-packet to a Client.</summary>
        /// <param name="client">ID of the Client</param>
        /// <param name="msg">Message to be sent</param>
        public static void Welcome(int client, string msg)
        {
            using var packet = new Packet((int) ServerPackets.Welcome);
            packet.Write(msg);
            packet.Write(client);
            SendTcpData(client, packet);
        }

        /// <summary>Sends a test-packet to a Client.</summary>
        /// <param name="client">ID of the Client</param>
        public static void UdpTest(int client)
        {
            using var packet = new Packet((int) ServerPackets.UdpTest);
            packet.Write("A test packet for UDP.");

            SendUdpData(client, packet);
        }

        #endregion
    }
}