using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class VoxelManagerClient : Manager<VoxelManagerClient>
{
    MapData mapData;
    GameObject mapObject;

    public void GenerateWorld(int seed)
    {
        mapData = MapGenerator.GenerateMapData(seed, GameResources.Get.MapGenerationSettings);
        mapObject = MapFabricator.FabricateMap(mapData, GameResources.Get.MapFabricationSettings);
        mapObject.name = "Map";
    }
}
