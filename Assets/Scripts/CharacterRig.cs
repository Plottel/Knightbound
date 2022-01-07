using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRig : MonoBehaviour
{
    public string meshPrefix;
    public TransformCache meshData;

    [HideInInspector]
    public List<Transform> meshes;

    private void OnDrawGizmos()
    {
        //var children = GetComponentsInChildren<Transform>();

        //for (int i = 0; i < children.Length; ++i)
        {
            //Vector3 pos = children[i].position;

            Vector3 pos = Vector3.zero;
            Debug.Log("Drawing Pos" + pos.ToString());

            var oldColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.FindTransform("Upper.Arm.L").position, 0.025f);
            Gizmos.DrawSphere(transform.FindTransform("Lower.Arm.L").position, 0.025f);
            Gizmos.DrawSphere(transform.FindTransform("Lower.Arm.L.end").position, 0.025f);

            Gizmos.DrawSphere(transform.FindTransform("Upper.Arm.R").position, 0.025f);
            Gizmos.DrawSphere(transform.FindTransform("Lower.Arm.R").position, 0.025f);
            Gizmos.DrawSphere(transform.FindTransform("Lower.Arm.R.end").position, 0.025f);

            Gizmos.color = oldColor;
        }
    }
}