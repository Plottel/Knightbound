using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : UIPanel
{
    Button hostBtn;
    Button quickJoinBtn;

    Button joinBtn;
    TMP_InputField joinInputField;

    public override void OnAwake()
    {
        base.OnAwake();

        hostBtn = transform.Find<RectTransform>("Host").Find<Button>("Button");
        quickJoinBtn = transform.Find<RectTransform>("QuickJoin").Find<Button>("Button");

        RectTransform joinContainer = transform.Find<RectTransform>("Join");
        joinBtn = joinContainer.Find<Button>("Button");
        joinInputField = joinContainer.Find<TMP_InputField>("InputField");

        hostBtn.onClick.AddListener(OnHostClicked);
        quickJoinBtn.onClick.AddListener(OnQuickJoinClicked);
        joinBtn.onClick.AddListener(OnJoinClicked);

        Debug.Log("Main Menu Awake");
    }

    void OnHostClicked()
    {
        string hostName = GameResources.hostName;
        ushort port = GameResources.port;

        // TODO: MOVE to a class, merge with same issue in ProgramEntryPoint
#if !UNITY_EDITOR
        ProgramEntryPoint.Get.InitializeServer();
#endif

        NetworkManagerServer.Get.LaunchServer(port);
        NetworkManagerClient.Get.JoinServer(hostName, port);
    }

    void OnJoinClicked()
    {
        string hostName = joinInputField.text;
        ushort port = GameResources.port;

        NetworkManagerClient.Get.JoinServer(hostName, port);
    }

    void OnQuickJoinClicked()
    {
        string hostName = GameResources.hostName;
        ushort port = GameResources.port;

        NetworkManagerClient.Get.JoinServer(hostName, port);
    }

    public override void OnVisibilityChanged(bool value)
    {
        base.OnVisibilityChanged(value);
        Debug.Log("MM Vis Changed - " + value.ToString());
    }
}
