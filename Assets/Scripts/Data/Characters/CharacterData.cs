using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Animator")]
    public AnimatorData animatorData;

    [Header("Armature")]
    public ArmatureData armatureData;
    public Transform armature;
    public CharacterMeshPiece[] defaultMeshPieces;
}