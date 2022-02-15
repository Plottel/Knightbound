using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class PerlinNoise
{
    public static float[,] GenerateMap(int seed, 
        int width,
        int depth,
        float scale, 
        int octaves, 
        float persistance, 
        float lacunarity, 
        Vector2 noiseOffset)
    {
        // Setup
        float[,] result = new float[width, depth];

        Random random = new Random(seed);

        float highestNoise = float.MinValue;
        float lowestNoise = float.MaxValue;
        float halfWidth = width / 2f;
        float halfDepth = depth / 2f;

        // Generate random Octave Offsets
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float octaveOffsetX = random.Next(-100000, 100000) + noiseOffset.x;
            float octaveOffsetY = random.Next(-100000, 100000) + noiseOffset.y;

            octaveOffsets[i] = new Vector2(octaveOffsetX, octaveOffsetY);
        }

        // Generate Noise Map
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
            {
                // Setup
                float amplitude = 1;
                float frequency = 1;
                float noiseValue = 1;

                // Affect Noise Value for each octave
                for (int i = 0; i < octaves; ++i)
                {
                    float octaveX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float octaveZ = (z - halfDepth) / scale * frequency + octaveOffsets[i].y;

                    noiseValue += Mathf.PerlinNoise(octaveX, octaveZ) * amplitude;

                    // Drop off for next octave
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // Noise Value calculated, update map.
                result[x, z] = noiseValue;

                // Did min/max noise values change?
                if (noiseValue > highestNoise) highestNoise = noiseValue;
                else if (noiseValue < lowestNoise) lowestNoise = noiseValue;
            }
        }

        // Map Calculated, normalize values between 0-1.
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
                result[x, z] = Mathf.InverseLerp(lowestNoise, highestNoise, result[x, z]);
        }

        return result;
    }
}
