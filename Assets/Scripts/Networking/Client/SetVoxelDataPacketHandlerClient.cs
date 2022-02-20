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
        MapData mapData = new MapData();
        TextureAtlas atlas = GameResources.Get.BlockAtlas;

        mapData.Deserialize(reader);
        Debug.Log("Deserializing MapData!" + "Width: " + mapData.width + " Depth: " + mapData.depth);

        VoxelManagerClient.Get.GenerateWorld(mapData);
    }
}
