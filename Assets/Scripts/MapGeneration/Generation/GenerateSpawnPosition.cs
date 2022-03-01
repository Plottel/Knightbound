using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpawnPosition : MapGenerationPass
{
    public int Padding = 5;

    public override void Execute(MapData data)
    {
        data.spawn = new Vector2Int
        {
            x = Random.Range(Padding, data.width - Padding),
            y = Random.Range(Padding, data.depth - Padding)
        };
    }
}