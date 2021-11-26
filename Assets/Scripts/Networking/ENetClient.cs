using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENet;
using System.IO;
using System.Text;

public class ENetClient
{
    private Host client;
    private Peer server;
    private Address serverAddress;

    private Action onConnectCallback;

    public void JoinServer(string hostName, ushort port, Action onConnect = null)
    {
        // This is where NAT Traversal needs to be implemented.
        var address = new Address();
        address.SetHost(hostName);
        address.Port = port;

        client = new Host();
        client.Create();


        onConnectCallback = onConnect;
        server = client.Connect(address);
    }

    public void SendPacket(MemoryStream stream)
    {
        Packet packet = new Packet();
        packet.Create(stream.GetBuffer());

        if (!server.Send(0, ref packet))
            Debug.Log("CLIENT: Failed to send packet");
    }

    public bool PumpPacket(MemoryStream packetStream)
    {
        ENet.Event netEvent;

        if (client.CheckEvents(out netEvent) <= 0)
        {
            if (client.Service(0, out netEvent) <= 0)
                return false;
        }

        switch (netEvent.Type)
        {
            case ENet.EventType.Connect:
                // Connection established.
                onConnectCallback?.Invoke();
                onConnectCallback = null;
                break;

            case ENet.EventType.Receive:
                // Our User-Generated packets will be Receive Packets
                using (BinaryWriter writer = new BinaryWriter(packetStream, Encoding.Default, true))
                {
                    Packet p = netEvent.Packet;
                    var packetData = new byte[p.Length];
                    p.CopyTo(packetData);

                    writer.Write(netEvent.Peer.IP);
                    writer.Write(packetData);
                }

                netEvent.Packet.Dispose();
                return true;
        }

        return false;
    }
}
