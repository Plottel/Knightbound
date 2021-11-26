using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using ENet;

public class ENetServer
{
    private Host server;
    private Dictionary<string, Peer> clients;

    private int nextClientID;

    public void LaunchServer(ushort port)
    {
        var address = new Address();
        address.Port = port;

        server = new Host();
        server.Create(address, 8);

        clients = new Dictionary<string, Peer>();
    }

    public void SendPacket(string ip, MemoryStream stream)
    {
        Packet packet = new Packet();
        packet.Create(stream.GetBuffer());

        Peer destination = clients[ip];

        if (!destination.Send(0, ref packet))
            Debug.Log("SERVER: Failed to send packet");
    }

    public void BroadcastPacket(MemoryStream stream)
    {
        Packet packet = new Packet();
        packet.Create(stream.GetBuffer());

        server.Broadcast(0, ref packet);
    }

    public bool PumpPacket(MemoryStream packetStream)
    {
        ENet.Event netEvent;

        if (server.CheckEvents(out netEvent) <= 0)
        {
            if (server.Service(0, out netEvent) <= 0)
            {
                return false;
            }
        }

        switch (netEvent.Type)
        {
            case ENet.EventType.Connect:

                return false;
                // This is the ENet Connection Packet for the initial handshake
                // If we receive this, we've successfully traversed the NAT.
                // The Game WelcomePacket will be sent as a Receive Packet.

            case ENet.EventType.Receive:
                string ip = netEvent.Peer.IP;

                // Add new Client
                if (!clients.ContainsKey(ip))
                    clients.Add(ip, netEvent.Peer);

                using (BinaryWriter writer = new BinaryWriter(packetStream, Encoding.Default, true))
                {
                    Packet p = netEvent.Packet;
                    var packetData = new byte[p.Length];
                    p.CopyTo(packetData);

                    writer.Write(ip);
                    writer.Write(packetData);
                }

                netEvent.Packet.Dispose();
                return true;
        }

        return false;
    }
}
