using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public static class NetworkPrefabRegistry
    {
        private static NetworkObject[] prefabs;

        public static void SetPrefabCount(int count) => prefabs = new NetworkObject[count];

        public static void Register(int typeID, NetworkObject prefab)
            => prefabs[typeID] = prefab;

        public static T Create<T>(int typeID) where T : NetworkObject
        {
            return Create(typeID) as T;   
        }

        public static NetworkObject Create(int typeID)
        {
            return Object.Instantiate(prefabs[typeID]);
        }
    }
}
