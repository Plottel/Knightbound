using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/MapGenerationSettings", menuName = "MapGenerationSettings")]
public class MapGenerationSettings : ScriptableObject
{
    public int size = 16;

    [SerializeReference]
    public List<MapGenerationPass> passes = new List<MapGenerationPass>();
}