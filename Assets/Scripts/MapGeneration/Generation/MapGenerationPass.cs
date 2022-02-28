using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MapGenerationPass
{
    public abstract void Execute(MapData data);
}
