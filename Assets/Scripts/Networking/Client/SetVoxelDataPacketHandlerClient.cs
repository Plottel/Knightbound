using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;
using System.IO;

public class SetVoxelDataPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var message = new GenerateMapMessage();
        TextureAtlas atlas = GameResources.Get.BlockAtlas;

        message.Deserialize(reader);

        VoxelManagerClient.Get.GenerateWorld(message.seed);
    }
}
