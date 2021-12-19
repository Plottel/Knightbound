using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class SmallManRigTest : MonoBehaviour
{
    public Transform leftShin;
    public Transform rightShin;

    public Transform leftThigh;
    public Transform rightThigh;

    public Transform groin;
    public Transform torso;

    public Transform upperArmLeft;
    public Transform upperArmRight;

    public Transform lowerArmLeft;
    public Transform lowerArmRight;

    public Transform head;

    private struct RigBinding
    {
        public Transform bone;
        public string meshName;
    }

    private List<RigBinding> rigBindings;
    public string meshPrefix = "Character_";
    public Transform[] meshes;

    private AnimancerComponent animancer;
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
        animancer.Play(idleAnimation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animancer.Stop();
            animancer.Play(walkAnimation);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animancer.Stop();
            animancer.Play(idleAnimation);
        }
    }

    private void AddRigBinding(Transform bone, string meshName)
    {
        rigBindings.Add(new RigBinding { bone = bone, meshName = meshName });
    }

    private void SetupBones()
    {
        leftShin = transform.FindTransform("Shin.L");
        rightShin = transform.FindTransform("Shin.R");
        leftThigh = transform.FindTransform("Thigh.L");
        rightThigh = transform.FindTransform("Thigh.R");
        groin = transform.FindTransform("Groin");
        torso = transform.FindTransform("Torso");
        upperArmLeft = transform.FindTransform("UpperArm.L");
        upperArmRight = transform.FindTransform("UpperArm.R");
        lowerArmLeft = transform.FindTransform("LowerArm.L");
        lowerArmRight = transform.FindTransform("LowerArm.R");
        head = transform.FindTransform("Head");
    }

    private void SetupRigBindings()
    {
        rigBindings = new List<RigBinding>();

        AddRigBinding(leftShin, "Shin_L");
        AddRigBinding(rightShin, "Shin_R");
        AddRigBinding(leftThigh, "Thigh_L");
        AddRigBinding(rightThigh, "Thigh_R");
        AddRigBinding(torso, "Torso");
        AddRigBinding(upperArmLeft, "Upper_Arm_L");
        AddRigBinding(upperArmRight, "Upper_Arm_R");
        AddRigBinding(lowerArmLeft, "Lower_Arm_L");
        AddRigBinding(lowerArmRight, "Lower_Arm_R");
        AddRigBinding(head, "Head");
    }

    public void AttachMeshes()
    {
        SetupBones();
        SetupRigBindings();

        if (meshes == null)
            return;

        foreach (RigBinding binding in rigBindings)
        {
            string meshName = meshPrefix + binding.meshName;

            if (FindMesh(meshName, out Transform mesh))
            {
                Transform bone = binding.bone;

                // Have the mesh and the bone
                // Set the mesh's position to the bone's position
                mesh.parent = bone.transform;
                mesh.position = bone.position;
            }
            else
                Debug.Log("Cannot find Mesh called " + meshName);
        }
    }

    private bool FindMesh(string meshName, out Transform outMesh)
    {
        outMesh = null;

        if (meshes == null)
            return false;

        foreach (Transform mesh in meshes)
        {
            if (mesh.name == meshName)
            {
                outMesh = mesh;
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.white;

        Vector3 cubeSize = new Vector3(0.05f, 0.05f, 0.05f);

        Gizmos.DrawCube(leftShin.position, cubeSize);
        Gizmos.DrawCube(rightShin.position, cubeSize);

        Gizmos.DrawLine(leftShin.position, leftThigh.position);
        Gizmos.DrawLine(leftShin.position, leftThigh.position);

        Gizmos.DrawLine(rightShin.position, rightThigh.position);
        Gizmos.DrawLine(rightShin.position, rightThigh.position);

        Gizmos.DrawLine(leftThigh.position, groin.position);
        Gizmos.DrawLine(rightThigh.position, groin.position);

        Gizmos.DrawCube(head.position, cubeSize);

        Gizmos.color = oldColor;
    }
}
