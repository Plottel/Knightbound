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
        var p = new ReplicationPacket();

        // Packet Header has been deserialized, but reader still contains object data.
        p.DeserializeHeader(reader);

        switch (p.action)
        {
            case ReplicationAction.Create: OnCreateActionReceived(p, reader); break;
            case ReplicationAction.Update: OnUpdateActionReceived(p, reader); break;
            case ReplicationAction.Destroy: OnDestroyActionReceived(p, reader); break;
        }
    }

    // Create the object if it doesn't exist, and update it.
    private void OnCreateActionReceived(ReplicationPacket p, BinaryReader reader)
    {
        NetworkObject obj;
        if (!context.TryGetNetworkObject(p.networkID, out obj))
        {
            obj = NetworkPrefabRegistry.Create(p.classID);
            context.RegisterNetworkObject(p.networkID, obj);
        }

        p.obj = obj;
        p.DeserializeBody(reader); // Deserialize object.
    }

    // Try to get the object and update it if it exists
    private void OnUpdateActionReceived(ReplicationPacket p, BinaryReader reader)
    {
        if (context.TryGetNetworkObject(p.networkID, out NetworkObject obj))
        {
            p.obj = obj;
            p.DeserializeBody(reader); // Deserialize object.
        }
    }

    // Try to get the object and destroy it if it exists
    private void OnDestroyActionReceived(ReplicationPacket p, BinaryReader reader)
    {
        context.DestroyNetworkOject(p.networkID);
        // No body to Deserialize...
    }
}
