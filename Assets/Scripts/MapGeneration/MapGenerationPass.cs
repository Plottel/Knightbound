using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationPass : MonoBehaviour
{
    public abstract void Execute(MapData data);
}
