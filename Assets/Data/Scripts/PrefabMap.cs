using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMap<T> : ScriptableObject
{
    [EnumArray(typeof(NetworkObjectType))]
    public T[] prefabs;
}
