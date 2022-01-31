using System;
using System.Collections;
using System.Collections.Generic;
using Deft;

public class GameManagerClient : Manager<GameManagerClient>
{
    public event Action eventBeginSimulation;

    //public override void OnStart()
    //{
    //    base.OnStart();
    //    StartCoroutine(JoinServer());
    //}

    //IEnumerator JoinServer()
    //{
    //    //yield return new WaitForSeconds(0.5f);
    //    yield return null;
    //    NetworkManagerClient.Get.JoinServer(hostName, port);
    //}

    public void BeginSimulation()
    {
        eventBeginSimulation?.Invoke();
    }
}
