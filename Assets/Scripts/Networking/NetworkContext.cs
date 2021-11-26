using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DestroyNetworkOject(int networkID)
    {
        // Will need to hook up to GameObject.OnDestroy somehow..
        NetworkObject obj = networkObjects[networkID];
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
}
