using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class GameManagerServer : Manager<GameManagerServer>
{
    private WorldData worldData;

    public override void OnStart()
    {
        base.OnStart();

        // We know this is a Listen Server. Prevent duplication to Local Client.
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);

        // Setup Server Simulation
        worldData = new WorldData();

        int worldSize = 64;
        worldData.blocks = new int[worldSize, worldSize];

        for (int x = 0; x < worldSize; ++x)
        {
            for (int z = 0; z < worldSize; ++z)
            {
                worldData.blocks[x, z] = Random.Range(1, 3);
            }
        }

        VoxelManagerServer.Get.GenerateWorld(worldData, GameResources.Get.BlockAtlas);
    }
}