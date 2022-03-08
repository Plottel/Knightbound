using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricateProp : MapFabricationPass
{
    static readonly int[] kRotations = new int[4]
    {
        0,
        90,
        180,
        270
    };


    public NetworkObjectType PropType;
    public bool RandomizeRotations;

    public override void Execute(MapData data, Map map)
    {
        int propID = (int)PropType;

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
            {
                if (data.propMap[x, z] == propID)
                {
                    var rms = ReplicationManagerServer.Get;
                    var prop = rms.CreateNetworkObject<Prop>(propID);

                    prop.transform.position = new Vector3(x, 0, z);

                    if (RandomizeRotations)
                    {
                        int yRotation = kRotations[Random.Range(0, 4)];
                        prop.transform.rotation = Quaternion.Euler(0, yRotation, 0);
                    }
                }
            }
        }

    }
}
