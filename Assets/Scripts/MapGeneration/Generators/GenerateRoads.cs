using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoads : MapGenerationPass
{
    public int RoadCount = 3;

    public override void Execute(MapData data)
    {
        Vector2Int spawn = data.spawn;
        data.roads = new LineSegment[RoadCount];
        for (int i = 0; i < RoadCount; ++i)
            data.roads[i] = new LineSegment
            {
                start = data.spawn,
                end = RandomPoint(data.width, data.depth)
            };
    }

    Vector2 RandomPoint(int width, int depth)
    {
        return new Vector2
        {
            x = Random.Range(0, width),
            y = Random.Range(0, depth)
        };
    }
}
