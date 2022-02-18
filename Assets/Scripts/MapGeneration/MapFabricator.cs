using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFabricator : MonoBehaviour
{
    public List<MapFabricationPass> passes = new List<MapFabricationPass>();

    public GameObject FabricateMap(MapData mapData)
    {
        var map = new GameObject("Map");

        foreach (MapFabricationPass pass in passes)
            pass.Execute(mapData, map);

        return map;
    }
}
