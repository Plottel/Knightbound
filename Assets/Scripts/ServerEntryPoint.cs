using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class ServerEntryPoint : Manager<ServerEntryPoint>
{
    private WorldData worldData;

    public override void OnStart()
    {
        // We know this is a Listen Server. Prevent duplication to Local Client.
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);

        // Setup Server Simulation
        worldData = WorldDataGenerator.GenerateWorldData(32);
        VoxelManagerServer.Get.GenerateWorld(worldData, GameResources.Get.BlockAtlas);
    }
}
