using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParrelSync;

public static class QuickStartMenuItems
{
    const string MENU = "QuickStart/";
    const string AUTO_HOST_JOIN = "AutoHostJoin";
    const string CLEAR = "Clear";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnStart()
    {
        bool autoHostJoin = EditorPrefs.GetBool(AUTO_HOST_JOIN);

        // Launch or Join server as appropriate
        if (autoHostJoin)
        {
            string hostName = GameResources.hostName;
            ushort port = GameResources.port;

            // The Original Instance hosts.
            if (!ClonesManager.IsClone())
                NetworkManagerServer.Get.LaunchServer(port);

            NetworkManagerClient.Get.JoinServer(hostName, port);

            return; // Prevent other QuickStarts executing.
        }
    }

    [MenuItem(MENU + AUTO_HOST_JOIN)]
    public static void EnableAutoHostJoin()
    {
        bool enabled = EditorPrefs.GetBool(AUTO_HOST_JOIN);
        EditorPrefs.SetBool(AUTO_HOST_JOIN, !enabled);
    }

    [MenuItem(MENU + AUTO_HOST_JOIN, true)]
    public static bool ValidateAutoHostJoin()
    {
        bool enabled = EditorPrefs.GetBool(AUTO_HOST_JOIN);
        Menu.SetChecked(MENU + AUTO_HOST_JOIN, enabled);
        return true;
    }

    [MenuItem(MENU + CLEAR)]
    public static void ClearQuickStart()
    {
        EditorPrefs.SetBool(AUTO_HOST_JOIN, false);
    }
}
