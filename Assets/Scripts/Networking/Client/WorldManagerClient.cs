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

    public void SetTerrainData(int row, int[] terrainData)
    {

    }

    public void GenerateWorld(int seed)
    {
        //mapData = MapGenerator.GenerateMapData(seed, GameResources.Get.MapGenerationSettings);
        //mapObject = MapFabricator.FabricateMap(mapData, GameResources.Get.MapFabricationSettings);
        //mapObject.name = "Map";
    }
}
