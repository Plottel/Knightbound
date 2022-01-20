using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerServer : GameManager<GameManagerServer>
{
    private ushort port = 6005;

    public override void OnAwake()
    {
        base.OnAwake();

        AddManager<NetworkManagerServer>();
        AddManager<PlayerManagerServer>();
        AddManager<InputManagerServer>();
        AddManager<ReplicationManagerServer>();
    }

    public override void OnStart()
    {
        base.OnStart();

        NetworkManagerServer.Get.LaunchServer(port);
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);
    }
}