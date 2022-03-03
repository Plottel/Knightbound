using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapFabricator
{
    public static Map FabricateMap(MapData mapData, MapFabricationSettings settings)
    {
        //var map = new GameObject("Map");
        var map = new Map();
        map.settings = new MapSettings
        {
            name = mapData.name,
            seed = mapData.seed,
            width = mapData.width,
            depth = mapData.depth
        };

        foreach (MapFabricationPass pass in settings.passes)
            pass.Execute(mapData, map);

        return map;
    }


}
