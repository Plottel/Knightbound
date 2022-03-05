using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft;
using Deft.Networking;

public class WorldManagerServer : Manager<WorldManagerServer>
{
    MapData mapData;
    Map map;

    public void GenerateWorld(int seed)
    {
        mapData = MapGenerator.GenerateMapData(seed, GameResources.Get.MapGenerationSettings);
        map = MapFabricator.FabricateMap(mapData, GameResources.Get.MapFabricationSettings);
    }

    public void SendMapSettings(int playerID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.SetMapSettings);
                map.settings.Serialize(writer);
            }
        }
    }

    public void SendTerrainData(int playerID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.SendTerrainData);
                writer.Write(mapData.width);
                writer.Write(mapData.depth);

                for (int x = 0; x < mapData.width; ++x)
                {
                    for (int z = 0; z < mapData.depth; ++z)
                    {
                        writer.Write(mapData.terrainMap[x, z]);
                    }
                }
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }

    void SendTerrainDataRow(int playerID, int row)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.SendTerrainData);
                writer.Write(mapData.width);
                writer.Write(row);

                for (int x = 0; x < mapData.width; ++x)
                    writer.Write(mapData.terrainMap[x, row]);
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }
}