using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/MapRenderSettings", menuName = "MapRenderSettings")]
public class MapRenderSettings : ScriptableObject
{
    [SerializeReference]
    public List<MapRenderPass> passes = new List<MapRenderPass>();
}
