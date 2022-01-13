using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds a Mesh and a bone ID to map to. 
[System.Serializable]
public class CharacterMeshPiece
{
    public Transform mesh;
    public int boneID;
}
