using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class ReplicationPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var command = new ReplicationCommand
        {
            action = (ReplicationAction)reader.ReadInt32(),
            classID = reader.ReadInt32(),
            networkID = reader.ReadInt32(),
            reader = reader
        };

        ReplicationManagerClient.Get.ProcessReplicationCommand(command);
    }
}
