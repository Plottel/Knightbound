using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : NoiseGenerator
{
    [Range(0, 1000)] 
    public float noiseScale = 250;
    [Range(0, 10)] 
    public int octaves = 5;
    [Range(0, 1)]
    public float persistence = 0.5f;
    [Range(1, 10)]
    public float lacunarity = 2;
    public Vector2 noiseOffset;

    public override float[,] GenerateMap(int seed, int width, int depth)
    {
        float[,] result = PerlinNoise.GenerateMap
        (
            seed + SeedOffset,
            width,
            depth,
            noiseScale,
            octaves,
            persistence,
            lacunarity,
            noiseOffset
        );

        return result;
    }
}
