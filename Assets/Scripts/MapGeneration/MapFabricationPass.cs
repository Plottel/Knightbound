using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MapFabricationPass
{
    public abstract void Execute(MapData data, Color[] pixels);
}
