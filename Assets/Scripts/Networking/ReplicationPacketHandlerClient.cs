using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplicationPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        client.replicator.ProcessReplicationPacket(reader);
        Debug.Log("CLIENT: Handling Replication Packet.");
    }
}
