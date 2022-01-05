using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft
{
    public class CharacterCamera : MonoBehaviour
    {
        public Transform target;
        public new Transform camera; // Component.camera is silly, redefining is fine.

        public void RotateY(float delta)
        {
            camera.RotateAround(target.position, Vector3.up, delta);
        }
    }
}
