namespace ServerTest
{
    public static class ClientSend
    {
        /// <summary>
        ///     Send TCP Data to a Client.
        /// </summary>
        /// <param name="packet">Data to be sent</param>
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.tcp.SendData(packet);
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