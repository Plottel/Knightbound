using System;
using UnityEngine;

namespace Deft
{
    public class Manager<T> : Singleton where T : MonoBehaviour
    {
        private static T _instance;

        public static T Get
        {
            get
            {
                return _instance;
            }
        }

        public override void EnsureInstance()
        {
            if (_instance == null)
                _instance = this as T;

            if (_instance == null)
                Debug.Log("Failed to set Singleton Instance: " + GetType().Name);
        }
    }
}
