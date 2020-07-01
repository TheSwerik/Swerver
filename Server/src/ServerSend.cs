using System.Linq;

namespace ServerTest
{
    public class ServerSend
    {
        /// <summary>
        ///     Send TCP Data to a Client.
        /// </summary>
        /// <param name="client">ID of the Client</param>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(int client, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[client].tcp.SendData(packet);
        }

        /// <summary>
        ///     Send TCP Data to all Clients.
        /// </summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            foreach (var client in Server.Clients.Values) client.tcp.SendData(packet);
        }

        /// <summary>
        ///     Send TCP Data to all Clients except one.
        /// </summary>
        /// <param name="packet">Data to be sent</param>
        /// <param name="except">ID of the Client that does not receive the Data</param>
        private static void SendTcpData(Packet packet, int except)
        {
            packet.WriteLength();
            foreach (var client in Server.Clients
                                         .Where(c => c.Key != except)
                                         .Select(a => a.Value)) client.tcp.SendData(packet);
        }

        public static void Welcome(int client, string msg)
        {
            using var packet = new Packet((int) ServerPackets.Welcome);
            packet.Write(msg);
            packet.Write(client);

            SendTcpData(client, packet);
        }
    }
}