using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    // Data
    [SerializeField]
    [HideInInspector]
    private AnimationClip[] animations;

    // Components
    private AnimancerComponent animancer;
    private Animator animator;
    
    private void FetchReferences()
    {
        animancer = GetComponent<AnimancerComponent>();
        animator = GetComponent<Animator>();

        animancer.Animator = animator;
    }

    void Awake()
    {
        FetchReferences();
    }

    private void OnValidate()
        => FetchReferences();

    public void Play(int animationID)
    {
        animancer.Play(animations[animationID]);
    }

    public void SetAnimatorController(RuntimeAnimatorController animatorController)
        => animator.runtimeAnimatorController = animatorController;

    public void SetAvatar(Avatar avatar)
    => animator.avatar = avatar;

    public void SetAnimations(AnimationClip[] newAnimations)
        => animations = newAnimations;
}