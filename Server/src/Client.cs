namespace ServerTest
{
    public class Client
    {
        private readonly int _id;
        public readonly Tcp Tcp;

        public Client(int id)
        {
            _id = id;
            Tcp = new ServerTcp(_id);
        }

        public class ServerTcp : Tcp
        {
            public ServerTcp(int id) : base(id) { }

            protected override void ExecuteOnMainThread(byte[] packetBytes, int id)
            {
                ThreadManager.ExecuteOnMainThread(() =>
                                                  {
                                                      using var packet = new Packet(packetBytes);
                                                      var packetId = packet.ReadInt();
                                                      Server.packetHandlers[packetId](Id, packet);
                                                  });
            }
        }
    }
}