using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSpawnPoint : MapRenderPass
{
    public int spawnPointRadius;
    public Color spawnPointColor;

    public override void Execute(MapData data, Color[] pixels)
    {
        for (int x = data.spawn.x - spawnPointRadius; x <= data.spawn.x + spawnPointRadius; ++x)
        {
            for (int z = data.spawn.y - spawnPointRadius; z <= data.spawn.y + spawnPointRadius; ++z)
            {
                int pixelIndex = z * data.depth + x;
                if (pixelIndex < 0 || pixelIndex >= pixels.Length)
                    continue;
                pixels[pixelIndex] = spawnPointColor;
            }
        }
    }
}
