using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft;
using Deft.Networking;

public class VoxelManagerServer : Manager<VoxelManagerServer>
{
    MapData mapData;
    GameObject mapObject;

    public void GenerateWorld(int seed)
    {
        mapData = MapGenerator.GenerateMapData(seed, GameResources.Get.MapGenerationSettings);
        mapObject = MapFabricator.FabricateMap(mapData, GameResources.Get.MapFabricationSettings);
        mapObject.name = "Map";
    }

    public void SendVoxelData(int playerID)
    {
        var message = new GenerateMapMessage
        {
            seed = mapData.seed,
        };

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.SetVoxelData);
                message.Serialize(writer);
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }
}
