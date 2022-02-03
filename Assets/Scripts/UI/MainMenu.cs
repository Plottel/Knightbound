using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIPanel
{
    Button hostBtn;
    Button quickJoinBtn;

    public override void OnAwake()
    {
        base.OnAwake();

        hostBtn = transform.Find<RectTransform>("Host").Find<Button>("Button");
        quickJoinBtn = transform.Find<RectTransform>("QuickJoin").Find<Button>("Button");

        hostBtn.onClick.AddListener(OnHostClicked);
        quickJoinBtn.onClick.AddListener(OnQuickJoinClicked);
    }

    void OnHostClicked()
    {
        string hostName = GameResources.hostName;
        ushort port = GameResources.port;

        NetworkManagerServer.Get.LaunchServer(port);
        NetworkManagerClient.Get.JoinServer(hostName, port);
    }

    void OnQuickJoinClicked()
    {
        string hostName = GameResources.hostName;
        ushort port = GameResources.port;
        NetworkManagerClient.Get.JoinServer(hostName, port);
    }
}
