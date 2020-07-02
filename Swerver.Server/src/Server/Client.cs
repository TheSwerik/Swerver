using Swerver.Util;

namespace Swerver.Server
{
    public class Client
    {
        public readonly int Id;
        internal readonly Tcp Tcp;
        internal readonly Udp Udp;

        public Client(int id)
        {
            Id = id;
            Tcp = new ServerTcp(Id);
            Udp = new Udp(Id);
        }

        private class ServerTcp : Tcp
        {
            public ServerTcp(int id) : base(id) { }

            protected override void ExecuteOnMainThread(byte[] packetBytes, int id)
            {
                ThreadManager.ExecuteOnMainThread(() =>
                                                  {
                                                      using var packet = new Packet(packetBytes);
                                                      var packetId = packet.ReadInt();
                                                      Server.PacketHandlers[packetId](Id, packet);
                                                  });
            }
        }
    }
}