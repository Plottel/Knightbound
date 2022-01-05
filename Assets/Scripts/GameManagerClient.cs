using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerClient : GameManager<GameManagerClient>
{
    private string hostName = "127.0.0.1";
    private ushort port = 6005;

    public override void OnAwake()
    {
        base.OnAwake();

        AddManager<NetworkManagerClient>();
        AddManager<CameraManager>();
    }

    public override void OnStart()
    {
        base.OnStart();
        StartCoroutine(JoinServer());
    }

    IEnumerator JoinServer()
    {
        yield return new WaitForSeconds(0.5f);
        NetworkManagerClient.Get.JoinServer(hostName, port);
    }
}
