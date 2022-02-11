using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldDataGenerator
{
    public static WorldData GenerateWorldData(int size)
    {
        var result = new WorldData();
        result.blocks = new int[size, size];

        for (int x = 0; x < size; ++x)
        {
            for (int z = 0; z < size; ++z)
            {
                result.blocks[x, z] = Random.Range(1, 3);
            }
        }

        return result;
    }
}
