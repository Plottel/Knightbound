using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public static class NetworkPrefabRegistry
    {
        private static NetworkObject[] prefabs;

        public static void SetPrefabCount(int count) => prefabs = new NetworkObject[count];

        public static void Register(NetworkObjectType type, NetworkObject prefab)
            => prefabs[(int)type] = prefab;

        public static T Create<T>(NetworkObjectType type) where T : NetworkObject
        {
            return Create(type) as T;   
        }

        public static NetworkObject Create(NetworkObjectType type)
        {
            return Object.Instantiate(prefabs[(int)type]);
        }
    }
}
