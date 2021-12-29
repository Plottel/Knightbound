using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deft.Networking
{
    public class NetworkContext
    {
        private Dictionary<int, NetworkObject> networkObjects;
        private Dictionary<NetworkObject, int> networkIDs;

        private int nextNetworkID;

        public NetworkContext()
        {
            nextNetworkID = 0;
            networkObjects = new Dictionary<int, NetworkObject>();
            networkIDs = new Dictionary<NetworkObject, int>();
        }

        public NetworkObject[] GetNetworkObjects()
        {
            return networkIDs.Keys.ToArray();
        }

        public void RegisterNetworkObject(int networkID, NetworkObject obj)
        {
            networkObjects[networkID] = obj;
            networkIDs[obj] = networkID;
        }

        public int RegisterNewNetworkObject(NetworkObject obj)
        {
            networkObjects[nextNetworkID] = obj;
            networkIDs[obj] = nextNetworkID;
            return nextNetworkID++;
        }

        public void DeregisterNetworkObject(int networkID)
        {
            NetworkObject obj = networkObjects[networkID];
            networkIDs.Remove(obj);
            networkObjects.Remove(networkID);
        }

        public void DeregisterNetworkObject(NetworkObject obj)
        {
            int networkID = networkIDs[obj];
            networkIDs.Remove(obj);
            networkObjects.Remove(networkID);
        }

        public bool TryGetNetworkObject(int networkID, out NetworkObject obj)
        {
            return networkObjects.TryGetValue(networkID, out obj);
        }

        public bool TryGetNetworkID(NetworkObject obj, out int networkID)
        {
            return networkIDs.TryGetValue(obj, out networkID);
        }

        public int GetNetworkID(NetworkObject obj)
        {
            return networkIDs[obj];
        }
    }
}
