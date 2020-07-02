using System.Net;
using ServerLibrary.Util;

namespace ServerLibrary.Server
{
    internal class Udp
    {
        private readonly int _id;
        internal IPEndPoint EndPoint;

        internal Udp(int id) { _id = id; }

        internal void Connect(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            ServerSend.UdpTest(_id);
        }

        internal void SendData(Packet packet) { Server.SendUdpData(EndPoint, packet); }

        internal void HandleData(Packet packetData)
        {
            var packetLength = packetData.ReadInt();
            var packetBytes = packetData.ReadBytes(packetLength);
            ThreadManager.ExecuteOnMainThread(() =>
                                              {
                                                  using var packet = new Packet(packetBytes);
                                                  var packetId = packet.ReadInt();
                                                  Server.PacketHandlers[packetId](_id, packet);
                                              });
        }
    }
}