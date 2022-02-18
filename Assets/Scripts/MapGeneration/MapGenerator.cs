using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapData GenerateMapData(int seed, MapGenerationSettings settings)
    {
        if (settings == null)
            return new MapData();

        var result = new MapData();

        Random.InitState(seed);

        result.seed = seed;
        result.width = settings.size;
        result.depth = settings.size;

        foreach (MapGenerationPass pass in settings.passes)
            pass.Execute(result);

        return result;
    }
}
