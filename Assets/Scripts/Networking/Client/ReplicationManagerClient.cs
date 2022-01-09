using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;
using Deft.Networking;

public class ReplicationManagerClient : Manager<ReplicationManagerClient>
{
    private NetworkReplicator replicator;

    public override void OnAwake()
    {
        base.OnAwake();
        replicator = new NetworkReplicator();
    }

    public void SetNetworkContext(NetworkContext context)
        => replicator.context = context;

    public void ProcessReplicationCommand(ReplicationCommand command)
        => replicator.ProcessReplicationCommand(command);
}
