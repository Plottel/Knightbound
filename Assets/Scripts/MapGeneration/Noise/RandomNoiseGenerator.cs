using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomNoiseGenerator : NoiseGenerator
{
    public override float[,] GenerateMap(int seed, int width, int depth)
    {
        float[,] result = new float[width, depth];

        Random random = new Random(seed + SeedOffset);

        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
                result[x, z] = (float)random.NextDouble();
        }

        return result;
    }
}
