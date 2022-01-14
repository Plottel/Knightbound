using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
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
        var characterData = character.characterData;
        Transform rootBone = Instantiate(characterData.armature);

        character.characterMesh.SetBoneData(rootBone, characterData.armatureData.boneNames);
    }

    void GenerateDefaultMeshes(Character character)
    {
        character.characterMesh.GenerateMesh(character.characterData.defaultMeshPieces);
    }

    void PrepareAnimator(Character character)
    {
        CharacterAnimator animator = character.characterAnimator;
        AnimatorData animatorData = character.characterData.animatorData;

        animator.SetAnimatorController(animatorData.animatorController);
        animator.SetAvatar(animatorData.avatar);
        animator.SetAnimations(animatorData.animations);
    }

    void ResetCharacter(Character character)
    {
        character.characterMesh.DestroyMeshes();
    }
}
