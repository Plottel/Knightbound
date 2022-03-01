using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRoads : MapRenderPass
{
    public Color RoadColor;

    public override void Execute(MapData data, Color[] pixels)
    {
        foreach (LineSegment road in data.roads)
        {
            var points = LineUtils.PointsOnLine(road.start, road.end);    

            foreach (Vector2 point in points)
            {
                pixels[(int)point.y * data.depth + (int)point.x] = RoadColor;
            }
        }
    }
}