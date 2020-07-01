namespace ServerTest
{
    public static class ClientSend
    {
        /// <summary>Send TCP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Tcp.SendData(packet);
        }

        /// <summary>Send UDP Data to the Server.</summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Udp.SendData(packet);
        }

        public static void WelcomeReceived()
        {
            using var packet = new Packet((int) ClientPackets.WelcomeReceived);
            packet.Write(Client.Instance.Id);
            packet.Write(MainWindow.Username);

            SendTcpData(packet);
        }
    }
}