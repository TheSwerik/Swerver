using Swerver.Util;

namespace Swerver.Client
{
    public static class ClientSend
    {
        #region Send Methods

        /// <summary>Send TCP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        public static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Tcp.SendData(packet);
        }

        /// <summary>Send UDP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        public static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Udp.SendData(packet);
        }

        #endregion

        #region Packets

        internal static void WelcomeReceived()
        {
            using var packet = new Packet((int) ClientPackets.WelcomeReceived);
            packet.Write(Client.Instance.Id);

            SendTcpData(packet);
        }

        internal static void UdpTestReceived()
        {
            using var packet = new Packet((int) ClientPackets.UdpTestReceived);
            packet.Write("Received a UDP Packet.");

            SendUdpData(packet);
        }

        #endregion
    }
}