using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateProp : MapGenerationPass
{
    public NetworkObjectType PropType;

    // TODO: Different distribution patterns - currently just random.
    [Range(0, 1)]
    public float Density = 0.02f;

    public override void Execute(MapData data)
    {
        int propID = (int)PropType;

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
            {
                if (Random.Range(0f, 1f) < Density)
                    data.propMap[x, z] = propID;
            }
        }
    }
}
