using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/AnimatorData", menuName = "AnimatorData")]
public class AnimatorData : ScriptableObject
{
    public RuntimeAnimatorController animatorController;
    public AnimationClip[] animations;
}
