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

            // Rotate the Camera around the point above player at Camera height
            //Vector3 rotationPoint = target.position;
            //rotationPoint.y = camera.position.y;

            //camera.RotateAround(target.position, Vector3.up, delta);
        }

        void SnapToTarget()
        {
            float height = target.position.y + CameraHeight;
            Vector3 back = target.forward * -1;
            back.y = 0;

            Vector3 rotatedBack = Quaternion.AngleAxis(cameraRotationOffset, Vector3.up) * back;

            // Player position
            // Add camera height in Y
            // X/Z by a vector with CameraRadius based on Target's Back vector
            // Get camera angle by "LookAt" target.

            Vector3 position = target.position;
            position.y += CameraHeight;
            position += rotatedBack * CameraRadius;

            // Set Position and Rotation
            camera.position = position;
            camera.LookAt(target, Vector3.up);
        }
    }
}
