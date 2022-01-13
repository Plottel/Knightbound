using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public ArmatureData armatureData;

    public Transform armature;
    public CharacterMeshPiece[] defaultMeshPieces;
}
