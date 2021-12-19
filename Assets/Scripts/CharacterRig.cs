using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRig : MonoBehaviour
{
    public string meshPrefix;
    public TransformCache meshData;

    [HideInInspector]
    public List<Transform> meshes;
}