using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class ReplicationPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        DeftNetworkManagerClient.Get.replicator.ProcessReplicationPacket(reader);
        Debug.Log("CLIENT: Handling Replication Packet.");
    }
}
