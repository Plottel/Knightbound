using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;
using System.IO;

public class SetTerrainDataPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        int width = reader.ReadInt32();
        int row = reader.ReadInt32();
        int[] terrainData = new int[width];

        for (int x = 0; x < width; ++x)
            terrainData[x] = reader.ReadInt32();





        //var message = new GenerateMapMessage();
        //TextureAtlas atlas = GameResources.Get.BlockAtlas;

        //message.Deserialize(reader);

        //WorldManagerClient.Get.GenerateWorld(message.seed);
    }
}
