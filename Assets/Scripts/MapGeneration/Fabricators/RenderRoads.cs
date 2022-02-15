using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRoads : MapFabricationPass
{
    public Color roadColor;

    public override void Execute(MapData data, Color[] pixels)
    {
        foreach (LineSegment road in data.roads)
        {
            var points = PointsOnLine(road.start, road.end);    

            foreach (Vector2 point in points)
            {
                pixels[(int)point.y * data.depth + (int)point.x] = roadColor;
            }
        }
    }

    // Bresenham Line Traversal
    List<Vector2> PointsOnLine(Vector2 start, Vector2 end)
    {
        var result = new List<Vector2>();

        bool isSteep = Mathf.Abs(end.y - start.y) > Mathf.Abs(end.x - start.x);

        if (isSteep)
        {
            Swap(ref start.x, ref start.y);
            Swap(ref end.x, ref end.y);
        }

        if (start.x > end.x)
        {
            Swap(ref start.x, ref end.x);
            Swap(ref start.y, ref end.y);
        }

        float dx = end.x - start.x;
        float dy = Mathf.Abs(end.y - start.y);

        float error = dx / 2.0f;
        int yStep = (start.y < end.y) ? 1 : -1;
        int y = (int)start.y;

        int maxX = (int)end.y;

        for (int x = (int)start.x; x < maxX; ++x)
        {
            if (isSteep)
                result.Add(new Vector2(y, x));
            else
                result.Add(new Vector2(x, y));

            error -= dy;

            if (error < 0)
            {
                y += yStep;
                error += dx;
            }
        }

        return result;
    }

    void Swap(ref float first, ref float second)
    {
        float temp = first;
        first = second;
        second = temp;
    }
}
