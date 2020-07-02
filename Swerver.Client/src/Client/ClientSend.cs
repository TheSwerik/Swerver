using ServerLibrary.Util;

namespace ServerLibrary.Client
{
    public static class ClientSend
    {
        /// <summary>Send TCP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Swerver.Client.Client.Client.Instance.Tcp.SendData(packet);
        }

        /// <summary>Send UDP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            Swerver.Client.Client.Client.Instance.Udp.SendData(packet);
        }

        public static void WelcomeReceived()
        {
            using var packet = new Packet((int) ClientPackets.WelcomeReceived);
            packet.Write(Swerver.Client.Client.Client.Instance.Id);
            packet.Write(MainWindow.Username);

            SendTcpData(packet);
        }

        public static void UdpTestReceived()
        {
            using var packet = new Packet((int) ClientPackets.UdpTestReceived);
            packet.Write("Received a UDP Packet.");

            SendUdpData(packet);
        }
    }
}