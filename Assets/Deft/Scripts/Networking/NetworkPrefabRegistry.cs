using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public static class NetworkPrefabRegistry
    {
        private static NetworkObject[] prefabs;

        public static void SetPrefabCount(int count) => prefabs = new NetworkObject[count];

        public static void Register(int type, NetworkObject prefab)
            => prefabs[type] = prefab;

        public static T Create<T>(int type) where T : NetworkObject
        {
            return Create(type) as T;   
        }

        public static NetworkObject Create(int type)
        {
            return Object.Instantiate(prefabs[type]);
        }
    }
}
