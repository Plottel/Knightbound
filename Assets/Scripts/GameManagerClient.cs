using System;
using System.Collections;
using System.Collections.Generic;

public class GameManagerClient : GameManager<GameManagerClient>
{
    public event Action eventBeginSimulation;

    private string hostName = "127.0.0.1";
    private ushort port = 6005;

    public override void OnAwake()
    {
        base.OnAwake();

        AddManager<NetworkManagerClient>();
        AddManager<PlayerManagerClient>();
        AddManager<InputManagerClient>();
        AddManager<InputDispatcherClient>();
        AddManager<ReplicationManagerClient>();
        AddManager<InputProcessorClient>();
        AddManager<VoxelManagerClient>();
        AddManager<CameraManager>();
    }

    public override void OnStart()
    {
        base.OnStart();
        StartCoroutine(JoinServer());
    }

    IEnumerator JoinServer()
    {
        //yield return new WaitForSeconds(0.5f);
        yield return null;
        NetworkManagerClient.Get.JoinServer(hostName, port);
    }

    public void BeginSimulation()
    {
        eventBeginSimulation?.Invoke();
    }
}
