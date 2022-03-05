using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class FabricateTerrain : MapFabricationPass
{
    public override void Execute(MapData data, Map map)
    {
        // Generate Mesh Game Object
        VoxelMesh voxelMesh = VoxelMeshGenerator.GenerateMesh(
            "TerrainMesh", 
            new Vector3(0, -0.5f, 0), 
            data.terrainMap, 
            GameResources.Get.BlockAtlas);

        map.terrain = voxelMesh;
    }
}
