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
        int depth = reader.ReadInt32();
        int[,] terrainMap = new int[width, depth];

        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
                terrainMap[x, z] = reader.ReadInt32();
        }

        WorldManagerClient.Get.SetTerrainData(terrainMap);
    }
}