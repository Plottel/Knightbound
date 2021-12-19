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

        public override void OnAwake()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                    Debug.Log("Instance doesnt exist");
            }


            else
                Debug.LogError("Instance already set. Whoops.");
        }
    }
}
