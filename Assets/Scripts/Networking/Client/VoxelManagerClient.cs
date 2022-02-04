using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class VoxelManagerClient : Manager<VoxelManagerClient>
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
}
