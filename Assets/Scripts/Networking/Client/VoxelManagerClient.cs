using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class VoxelManagerClient : Manager<VoxelManagerClient>
{
    MapData mapData;
    GameObject mapObject;

    public void GenerateWorld(MapData data)
    {
        mapData = data;
        mapObject = MapFabricator.FabricateMap(mapData, GameResources.Get.MapFabricationSettings);
        mapObject.name = "Map";
    }
}
