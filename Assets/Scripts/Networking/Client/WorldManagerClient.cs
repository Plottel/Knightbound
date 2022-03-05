using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class WorldManagerClient : Manager<WorldManagerClient>
{
    Map map;

    public override void OnAwake()
    {
        map = new Map();
    }

    public void SetMapSettings(MapSettings settings)
    {
        map.settings = settings;
    }

    public void SetTerrainData(int[,] terrainData)
    {
        // Generate Mesh Game Object
        VoxelMesh voxelMesh = VoxelMeshGenerator.GenerateMesh(
            "TerrainMesh",
            new Vector3(0, -0.5f, 0),
            terrainData,
            GameResources.Get.BlockAtlas);

        // Destroy Old Mesh
        if (map.terrain != null)
            Destroy(map.terrain.gameObject);

        map.terrain = voxelMesh;
    }
}
