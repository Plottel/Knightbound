using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Character character = target as Character;

        if (GUILayout.Button("Full Setup"))
            SetupCharacter(character);

        if (GUILayout.Button("Generate Skeleton"))
            GenerateSkeleton(character);

        if (GUILayout.Button("Generate Default Meshes"))
            GenerateDefaultMeshes(character);

        if (GUILayout.Button("Prepare Animator"))
            PrepareAnimator(character);

        if (GUILayout.Button("Reset Character"))
            ResetCharacter(character);
    }

    void SetupCharacter(Character character)
    {
        GenerateSkeleton(character);
        GenerateDefaultMeshes(character);
        PrepareAnimator(character);
    }

    void GenerateSkeleton(Character character)
    {
        var characterData = character.data;
        Transform rootBone = Instantiate(characterData.armature);

        character.mesh.SetBoneData(rootBone, characterData.armatureData.boneNames);
    }

    void GenerateDefaultMeshes(Character character)
    {
        character.mesh.GenerateMesh(character.data.defaultMeshPieces);
    }

    void PrepareAnimator(Character character)
    {
        CharacterAnimator animator = character.animator;
        AnimatorData animatorData = character.data.animatorData;

        animator.SetAnimatorController(animatorData.animatorController);
        animator.SetAvatar(animatorData.avatar);
        animator.SetAnimations(animatorData.animations);
    }

    void ResetCharacter(Character character)
    {
        character.mesh.DestroyMeshes();
    }
}
