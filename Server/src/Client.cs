using System.Net;

namespace ServerTest
{
    public class Client
    {
        private readonly int _id;
        public readonly Tcp Tcp;
        public readonly Udp udp;

        public Client(int id)
        {
            _id = id;
            Tcp = new ServerTcp(_id);
            udp = new Udp(_id);
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

        public class Udp
        {
            private readonly int _id;
            public IPEndPoint EndPoint;

            public Udp(int id) { _id = id; }

            public void Connect(IPEndPoint endPoint)
            {
                EndPoint = endPoint;
                ServerSend.UdpTest(_id);
            }

            public void SendData(Packet packet) { Server.SendUDPData(EndPoint, packet); }

            public void HandleData(Packet packetData)
            {
                var packetLength = packetData.ReadInt();
                var packetBytes = packetData.ReadBytes(packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                                                  {
                                                      using var packet = new Packet(packetBytes);
                                                      var packetId = packet.ReadInt();
                                                      Server.packetHandlers[packetId](_id, packet);
                                                  });
            }
        }
    }
}