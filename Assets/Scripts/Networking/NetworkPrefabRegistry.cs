using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public static class NetworkPrefabRegistry
    {
        public static T Create<T>(NetworkObjectType type) where T : NetworkObject
        {
            switch (type)
            {
                case NetworkObjectType.Tree: return CreateTree() as T;
            }

            return null;
        }

        public static NetworkObject Create(NetworkObjectType type)
        {
            switch (type)
            {
                case NetworkObjectType.Tree: return CreateTree();
            }

            return null;
        }

        private static Tree CreateTree()
        {
            return new Tree();
        }
    }
}
