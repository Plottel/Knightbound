using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapFabricationPass : MonoBehaviour
{
    public abstract void Execute(MapData data, Color[] pixels);
}
