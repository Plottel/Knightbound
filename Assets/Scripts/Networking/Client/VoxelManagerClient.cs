using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class VoxelManagerClient : Manager<VoxelManagerClient>
{
    private VoxelWorldData worldData;
    private TextureAtlas atlas;

    private VoxelMesh voxelMesh;

    public void GenerateWorld(VoxelWorldData newWorldData, Texture2D[] textures)
    {
        // Set Data
        worldData = newWorldData;
        atlas = new TextureAtlas(textures); // TODO: BAD! Don't create Atlas here!

        // Generate Mesh Game Object
        voxelMesh = VoxelMeshGenerator.GenerateMesh(worldData, atlas);
        voxelMesh.transform.parent = transform;
        voxelMesh.transform.position = new Vector3(0, -0.5f, 0);
    }
}
