using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMap<T> : ScriptableObject
{
    [NamedEnumArray(typeof(NetworkObjectType))]
    public T[] prefabs;
}
