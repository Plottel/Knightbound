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
        WorldData worldData = new WorldData();
        TextureAtlas atlas = GameResources.Get.BlockAtlas;

        worldData.Deserialize(reader);

        VoxelManagerClient.Get.GenerateWorld(worldData, atlas);
    }
}
