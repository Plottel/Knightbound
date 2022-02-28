using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class FabricateTerrain : MapFabricationPass
{
    public override void Execute(MapData data, GameObject root)
    {
        TextureAtlas atlas = GameResources.Get.BlockAtlas;

        // Generate Mesh Game Object
        VoxelMesh voxelMesh = VoxelMeshGenerator.GenerateMesh(data.terrainMap, atlas);
        voxelMesh.transform.parent = root.transform;
        voxelMesh.transform.position = new Vector3(0, -0.5f, 0);
        voxelMesh.name = "TerrainMesh";
    }
}
