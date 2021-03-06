﻿using System;
using System.Net;
using System.Net.Sockets;

namespace Swerver.Util
{
    public abstract class Udp
    {
        private IPEndPoint _endPoint;
        internal UdpClient Socket;
        internal void Init(string ip, int port) { _endPoint = new IPEndPoint(IPAddress.Parse(ip), port); }

        internal void Connect(int localPort)
        {
            Socket = new UdpClient(localPort);

            Socket.Connect(_endPoint);

            Socket.BeginReceive(ReceiveCallback, null);

            using var packet = new Packet();
            SendData(packet);
        }

        private void Disconnect()
        {
            Client.Client.Instance.Disconnect();
            _endPoint = null;
            Socket = null;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (!Client.Client.Instance.IsConnected) return;
                var data = Socket.EndReceive(result, ref _endPoint);
                Socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    Client.Client.Instance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving Server Data: {e}");
                Disconnect();
            }
        }

        internal void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(Client.Client.Instance.Id);
                Socket?.BeginSend(packet.ToArray(), packet.Length(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Error sending data to Player {Client.Client.Instance.Id} via Udp: {e}");
            }
        }

        private void HandleData(byte[] data)
        {
            using var pkt = new Packet(data);

            var packetLength = pkt.ReadInt();
            data = pkt.ReadBytes(packetLength);

            ExecuteOnMainThread(() =>
                                {
                                    using var packet = new Packet(data);
                                    var packetId = packet.ReadInt();
                                    Client.Client.PacketHandlers[packetId](packet);
                                });
        }

        protected abstract void ExecuteOnMainThread(Action action);
    }
}