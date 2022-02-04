using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft;
using Deft.Networking;

public class VoxelManagerServer : Manager<VoxelManagerServer>
{
    private WorldData worldData;
    private TextureAtlas atlas;

    private WorldMesh voxelMesh;

    public void GenerateWorld(WorldData newWorldData, TextureAtlas newAtlas)
    {
        // Set Data
        worldData = newWorldData;
        atlas = newAtlas;

        // Generate Mesh Game Object
        voxelMesh = VoxelMeshGenerator.GenerateMesh(worldData, atlas);
        voxelMesh.transform.parent = transform;
        voxelMesh.transform.position = new Vector3(0, -0.5f, 0);
    }

    public void SendVoxelData(int playerID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.SetVoxelData);
                worldData.Serialize(writer);
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }
}
