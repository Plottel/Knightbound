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
    private Dictionary<ClientInfo, NetworkReplicator> clientReplicators;

    private Dictionary<int, ClientInfo> playerIDToClientInfo;

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
        clientReplicators = new Dictionary<ClientInfo, NetworkReplicator>();

        playerIDToClientInfo = new Dictionary<int, ClientInfo>();

        NetworkManagerServer.Get.eventPlayerJoined += OnPlayerJoined;
    }

    void OnDestroy() => NetworkManagerServer.Get.eventPlayerJoined -= OnPlayerJoined;

    private void OnPlayerJoined(PlayerInfo playerInfo)
    {
        clientReplicators.Add(playerInfo.clientInfo, new NetworkReplicator());
        playerIDToClientInfo.Add(playerInfo.clientInfo.playerID, playerInfo.clientInfo);
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

    private void SynchronizeClientReplicators()
    {
        var serverObjects = serverReplicator.GetNetworkObjects();

        foreach (var clientReplicationInfo in clientReplicators)
        {
            ClientInfo client = clientReplicationInfo.Key;
            NetworkReplicator clientReplicator = clientReplicationInfo.Value;

            foreach (NetworkObject serverObject in serverObjects)
            {
                // If Client has this Network ID, Update the existing object
                if (clientReplicator.context.TryGetNetworkID(serverObject, out int networkID))
                {
                    SendUpdate(client, networkID, serverObject);
                }
                // Client does not have Network ID, Create new obj using existing Network ID.
                else
                {
                    networkID = serverReplicator.context.GetNetworkID(serverObject);
                    clientReplicator.context.RegisterNetworkObject(networkID, serverObject);

                    SendCreate(client, networkID, serverObject);
                }
            }
        }
    }

    private void SendCreate(ClientInfo client, int networkID, NetworkObject obj)
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

            NetworkManagerServer.Get.SendPacket(client.peerID, stream);
        }
    }

    private void SendUpdate(ClientInfo client, int networkID, NetworkObject obj)
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

            NetworkManagerServer.Get.SendPacket(client.peerID, stream);
        }
    }

    public T CreateNetworkObject<T>(int classID, out int networkID) where T : NetworkObject
    {
        var newObj = NetworkPrefabRegistry.Create<T>(classID);
        networkID = serverReplicator.context.RegisterNewNetworkObject(newObj);

        return newObj;
    }

    // NOTE: Cant pass T to TryGetValue...
    public bool TryGetNetworkObject(int networkID, out NetworkObject networkObject)
    {
        return serverReplicator.context.TryGetNetworkObject(networkID, out networkObject);
    }
}