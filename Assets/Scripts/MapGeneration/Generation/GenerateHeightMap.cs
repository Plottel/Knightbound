 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHeightMap : MapGenerationPass
{
    [SerializeReference]
    public PerlinNoiseGenerator perlinNoise;

    public override void Execute(MapData data)
    {
        data.heightMap = perlinNoise.GenerateMap(data.seed, data.width, data.depth);
    }
}
