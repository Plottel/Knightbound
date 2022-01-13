using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/TransformCache", menuName = "TransformCache")]
public class TransformCache : ScriptableObject
{
    public Transform[] transforms;
}
