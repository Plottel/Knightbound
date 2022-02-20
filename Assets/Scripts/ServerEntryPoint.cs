using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class ServerEntryPoint : Manager<ServerEntryPoint>
{
    public override void OnStart()
    {
        // We know this is a Listen Server. Prevent duplication to Local Client.
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);

        // Setup Server Simulation
        int seed = Random.Range(0, int.MaxValue);
        VoxelManagerServer.Get.GenerateWorld(seed);
    }
}
