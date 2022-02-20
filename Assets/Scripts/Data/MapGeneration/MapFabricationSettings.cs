using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/MapFabricationSettings", menuName = "MapFabricationSettings")]
public class MapFabricationSettings : ScriptableObject
{
    [SerializeReference]
    public List<MapFabricationPass> passes = new List<MapFabricationPass>();
}
