using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSpawnPoint : MapRenderPass
{
    public int SpawnPointRadius;
    public Color SpawnPointColor;

    public override void Execute(MapData data, Color[] pixels)
    {
        for (int x = data.spawn.x - SpawnPointRadius; x <= data.spawn.x + SpawnPointRadius; ++x)
        {
            for (int z = data.spawn.y - SpawnPointRadius; z <= data.spawn.y + SpawnPointRadius; ++z)
            {
                int pixelIndex = z * data.depth + x;
                pixels[pixelIndex] = SpawnPointColor;
            }
        }
    }
}
