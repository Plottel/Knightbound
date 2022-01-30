using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class GameManagerServer : Manager<GameManagerServer>
{
    private ushort port = 6005;

    private VoxelWorldData worldData;

    public override void OnStart()
    {
        base.OnStart();

        // We know this is a Listen Server. Prevent duplication to Local Client.
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);

        // Setup Server Simulation
        worldData = new VoxelWorldData();
        worldData.voxelData = new int[,]
        {
            {0, 1, 1, 1, 1, 0},
            {1, 2, 2, 2, 2, 1},
            {1, 2, 2, 2, 2, 1},
            {1, 2, 2, 2, 2, 1},
            {1, 2, 2, 2, 2, 1},
            {0, 1, 1, 1, 1, 0},
        };

        VoxelManagerServer.Get.GenerateWorld(worldData, GameResources.Get.BlockTextures);

        // Launch Server
        NetworkManagerServer.Get.LaunchServer(port);
    }
}