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
        Texture2D[] textures = GameResources.Get.BlockTextures;

        worldData.Deserialize(reader);

        VoxelManagerClient.Get.GenerateWorld(worldData, textures);
    }
}
