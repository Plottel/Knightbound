using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

[RequireComponent(typeof(AnimancerComponent))]
public class CharacterAnimator : MonoBehaviour
{
    private AnimancerComponent animator;
    private AnimationClip[] animations;

    void Awake()
    {
        animator = GetComponent<AnimancerComponent>();
    }

    public void Play(int animationID)
    {
        animator.Play(animations[0]);
    }

    public void SetAnimatorController(RuntimeAnimatorController animatorController)
    {
        // TODO: Fix - properly define relationship between FBX Bones and Armature
        GetComponentInChildren<Animator>().runtimeAnimatorController = animatorController;
    }

    public void SetAnimations(AnimationClip[] newAnimations)
    {
        animations = newAnimations;
    }
}
