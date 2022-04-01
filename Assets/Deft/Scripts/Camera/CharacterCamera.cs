using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft
{
    public class CharacterCamera : MonoBehaviour
    {
        public float CameraHeight = 20f;
        public float CameraRadius = 10f;

        private float cameraRotationOffset = 0f;

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

        public void RotateCamera(float delta)
        {
            cameraRotationOffset += delta;
        }

        void SnapToTarget()
        {
            // Get Player position
            Vector3 cameraPosition = target.position;

            // Go up by Camera Height
            cameraPosition.y += CameraHeight;

            // Get Angle Vector to project out by radius
            Vector3 zeroAngle = new Vector3(1, 0, 0);
            Vector3 cameraAngle = Quaternion.AngleAxis(cameraRotationOffset, Vector3.up) * zeroAngle;

            // Move out along Angle Vector by Camera Radius
            cameraPosition += cameraAngle * CameraRadius;

            // Set Camera Position
            camera.position = cameraPosition;

            // Set Camera Rotation
            camera.LookAt(target, Vector3.up);
        }
    }
}
