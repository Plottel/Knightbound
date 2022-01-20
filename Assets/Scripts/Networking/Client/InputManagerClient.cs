using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class InputManagerClient : Manager<InputManagerClient>
{
    private PlayerControllerClient controller;

    public override void OnAwake()
    {
        base.OnAwake();
        controller = new PlayerControllerClient();
    }

    public override void OnUpdate()
    {
        // TODO: Initialize these Managers in response to NetworkState becoming Playing?
        if (NetworkManagerClient.Get.state == NetworkState.Playing)
            controller.OnUpdate();
    }
}
