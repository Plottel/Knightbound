using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapFabricator
{
    public static GameObject FabricateMap(MapData mapData, MapFabricationSettings settings)
    {
        var map = new GameObject("Map");

        foreach (MapFabricationPass pass in settings.passes)
            pass.Execute(mapData, map);

        return map;
    }


}
