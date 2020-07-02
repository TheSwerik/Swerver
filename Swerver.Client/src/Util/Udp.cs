using System;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using ServerLibrary.Util;

namespace ServerLibrary.Client
{
   
        public abstract class Udp
        {
            private IPEndPoint _endPoint;
            private UdpClient _socket;

            protected Udp(string ip, int port) { _endPoint = new IPEndPoint(IPAddress.Parse(ip), port); }

            internal void Connect(int localPort)
            {
                _socket = new UdpClient(localPort);

                _socket.Connect(_endPoint);

                _socket.BeginReceive(ReceiveCallback, null);

                using var packet = new Packet();
                SendData(packet);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var data = _socket.EndReceive(result, ref _endPoint);
                    _socket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4)
                        //TODO Disconnect
                        return;

                    HandleData(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving Server Data: {e}");
                    //TODO Disconnect
                }
            }

            internal void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(Client.Instance.Id);
                    _socket?.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data to Player {Client.Instance.Id} via Udp: {e}");
                }
            }

            private void HandleData(byte[] data)
            {
                using var pckt = new Packet(data);

                var packetLength = pckt.ReadInt();
                data = pckt.ReadBytes(packetLength);

                ExecuteOnMainThread(() =>
                                    {
                                        using var packet = new Packet(data);
                                        var packetId = packet.ReadInt();
                                        Client.PacketHandlers[packetId](packet);
                                    });
            }

            protected abstract void ExecuteOnMainThread(Action action);
        }
}