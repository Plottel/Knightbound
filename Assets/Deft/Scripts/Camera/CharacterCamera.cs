using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft
{
    public class CharacterCamera : MonoBehaviour
    {
        public Vector3 TargetOffset;

        public Transform target { get; private set; }
        public new Transform camera; // Component.camera is silly, redefining is fine.

        private void Update()
        {
            if (target == null)
                return;

            SnapToTarget();
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            SnapToTarget();
        }

        public void SetDefaultRotation(float x, float y, float z)
        {
            camera.rotation = Quaternion.Euler(x, y, z);
        }

        public void RotateAroundTarget(float delta)
        {
            // Rotate the Camera around the point above player at Camera height
            Vector3 rotationPoint = target.position;
            rotationPoint.y = camera.position.y;

            camera.RotateAround(rotationPoint, Vector3.up, delta);
        }

        void SnapToTarget()
        {
            camera.position = target.position + TargetOffset;
        }
    }
}
