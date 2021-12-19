using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDebug : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.zero, 0.05f);
    }
}
