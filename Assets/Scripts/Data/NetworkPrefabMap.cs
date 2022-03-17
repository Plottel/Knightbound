using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft.Networking;

[CreateAssetMenu(fileName = "Data/NetworkPrefabMap", menuName = "NetworkPrefabMap")]
public class NetworkPrefabMap : ScriptableObject
{
    public NetworkObject[] prefabs;
}
