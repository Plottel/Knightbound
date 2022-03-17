using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderProp : MapRenderPass
{
    public PrefabID PropType;
    public Color PropColor = new Color(1, 0, 1);

    public override void Execute(MapData data, Color[] pixels)
    {
        int propID = (int)PropType;

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
            {
                if (data.propMap[x, z] == propID)
                    pixels[z * data.depth + x] = PropColor;
            }
        }
    }
}
