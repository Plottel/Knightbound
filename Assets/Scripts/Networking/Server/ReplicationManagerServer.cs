using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft;
using Deft.Networking;

public class ReplicationManagerServer : Manager<ReplicationManagerServer>
{
    private NetworkReplicator serverReplicator; 
    private Dictionary<int, NetworkReplicator> playerIDToReplicator;

    private const float kUpdateSendInterval = 0.033f;
    private float timeSinceLastUpdate;

    public NetworkContext NetworkContext
    {
        get => serverReplicator.context;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        serverReplicator = new NetworkReplicator();
        playerIDToReplicator = new Dictionary<int, NetworkReplicator>();
    }

    public override void OnLateUpdate()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate > kUpdateSendInterval)
        {
            timeSinceLastUpdate = 0f;
            SynchronizeClientReplicators();
        }
    }

    public void RegisterPlayer(int playerID)
    {
        playerIDToReplicator[playerID] = new NetworkReplicator();
    }

    public void SendFullSync(int playerID)
    {
        NetworkReplicator playerReplicator = playerIDToReplicator[playerID];

        foreach (NetworkObject serverObject in serverReplicator.GetNetworkObjects())
        {
            if (playerReplicator.context.TryGetNetworkID(serverObject, out int networkID))
                SendUpdate(playerID, networkID, serverObject);
            else
            {
                networkID = serverReplicator.context.GetNetworkID(serverObject);
                playerReplicator.context.RegisterNetworkObject(networkID, serverObject);

                SendCreate(playerID, networkID, serverObject);
            }
        }
    }

    private void SynchronizeClientReplicators()
    {
        var serverObjects = serverReplicator.GetNetworkObjects();

        foreach (var playerReplicationInfo in playerIDToReplicator)
        {
            int playerID = playerReplicationInfo.Key;
            NetworkReplicator playerReplicator = playerReplicationInfo.Value;

            foreach (NetworkObject serverObject in serverObjects)
            {
                // If Client has this Network ID, Update the existing object
                if (playerReplicator.context.TryGetNetworkID(serverObject, out int networkID))
                {
                    if (serverObject.ShouldSendUpdate())
                        SendUpdate(playerID, networkID, serverObject);
                }
                // Client does not have Network ID, Create new obj using existing Network ID.
                else
                {
                    networkID = serverReplicator.context.GetNetworkID(serverObject);
                    playerReplicator.context.RegisterNetworkObject(networkID, serverObject);

                    SendCreate(playerID, networkID, serverObject);
                }
            }
        }
    }

    private void SendCreate(int playerID, int networkID, NetworkObject obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Create);
                writer.Write(obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }

    private void SendUpdate(int playerID, int networkID, NetworkObject obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Update);
                writer.Write((int)obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            NetworkManagerServer.Get.SendPacket(playerID, stream);
        }
    }

    public T CreateNetworkObject<T>(int classID, out int networkID) where T : NetworkObject
    {
        var newObj = NetworkPrefabRegistry.Create<T>(classID);
        networkID = serverReplicator.context.RegisterNewNetworkObject(newObj);

        return newObj;
    }

    public T CreateNetworkObject<T>(int classID) where T : NetworkObject
        => CreateNetworkObject<T>(classID, out int networkID);

    // NOTE: Cant pass T to TryGetValue...
    public bool TryGetNetworkObject(int networkID, out NetworkObject networkObject)
    {
        return serverReplicator.context.TryGetNetworkObject(networkID, out networkObject);
    }
}