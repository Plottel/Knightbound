using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class NetworkReplicator
{
    public NetworkContext context;

    public NetworkReplicator()
    {
        context = new NetworkContext();
    }

    public void ProcessReplicationPacket(BinaryReader reader)
    {
        var action = (ReplicationAction)reader.ReadInt32();

        switch (action)
        {
            case ReplicationAction.Create: OnCreateActionReceived(reader); break;
            case ReplicationAction.Update: OnUpdateActionReceived(reader); break;
            case ReplicationAction.Destroy: OnDestroyActionReceived(reader); break;
        }
    }

    public NetworkObject[] GetNetworkObjects()
    {
        return context.GetNetworkObjects();
    }

    // Create the object if it doesn't exist, and update it.
    private void OnCreateActionReceived(BinaryReader reader)
    {
        var classID = (NetworkObjectType)reader.ReadInt32();
        int networkID = reader.ReadInt32();

        NetworkObject obj;
        if (!context.TryGetNetworkObject(networkID, out obj))
        {
            obj = NetworkPrefabRegistry.Create(classID);
            context.RegisterNetworkObject(networkID, obj);
        }

        obj.Deserialize(reader); // Deserialize object.
    }

    // Try to get the object and update it if it exists
    private void OnUpdateActionReceived(BinaryReader reader)
    {
        var classID = (NetworkObjectType)reader.ReadInt32();
        int networkID = reader.ReadInt32();

        NetworkObject obj;
        if (!context.TryGetNetworkObject(networkID, out obj))
        {
            obj = NetworkPrefabRegistry.Create(classID); // DON'T register dummy.
            obj.Deserialize(reader); // Advance stream regardless of if we have the object.
            Object.Destroy(obj.gameObject);
        }
        else // Object already exists
        {
            obj.Deserialize(reader);
        }

    }

    // Try to get the object and destroy it if it exists
    private void OnDestroyActionReceived(BinaryReader reader)
    {
        int networkID = reader.ReadInt32();

        if (context.TryGetNetworkObject(networkID, out NetworkObject obj))
        {
            context.DeregisterNetworkObject(networkID);
            Object.Destroy(obj.gameObject);
        }
    }
}
