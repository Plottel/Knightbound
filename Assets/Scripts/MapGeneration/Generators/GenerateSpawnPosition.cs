using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpawnPosition : MapGenerationPass
{
    public override void Execute(MapData data)
    {
        data.spawn = new Vector2Int
        {
            x = Random.Range(0, data.width),
            y = Random.Range(0, data.depth)
        };
    }
}
