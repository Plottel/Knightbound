using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;
using Deft.Networking;
using ParrelSync;

public class GameManager : MonoBehaviour
{
    public NetworkObjectMap NetworkPrefabs;

    private string hostName = "127.0.0.1";
    private ushort port = 6005;

    private List<Singleton> managers;

    void Awake()
    {
        // Setup Game Resources
        var prefabs = NetworkPrefabs.prefabs;
        NetworkPrefabRegistry.SetPrefabCount(prefabs.Length);

        // Register Netowrk Prefabs
        for (int i = 0; i < prefabs.Length; i++)
            NetworkPrefabRegistry.Register((NetworkObjectType)i, prefabs[i]);

        // Setup Networking
        ENet.Library.Initialize();

        // Create Managers
        managers = new List<Singleton>();

        AddManager<NetworkManagerClient>();

        if (!ClonesManager.IsClone())
            AddManager<NetworkManagerServer>();

        foreach (var manager in managers)
            manager.OnAwake();
    }

    private void Start()
    {
        // The original makes the server.
        if (!ClonesManager.IsClone())
        {
            NetworkManagerServer.Get.LaunchServer(port);
            NetworkManagerClient.Get.SetContext(NetworkManagerServer.Get.GetContext());
        }

        NetworkManagerClient.Get.JoinServer(hostName, port);

        foreach (var manager in managers)
            manager.OnStart();
    }

    void Update()
    {
        foreach (var manager in managers)
            manager.OnUpdate();
    }

    void LateUpdate()
    {
        foreach (var manager in managers)
            manager.OnLateUpdate();
    }

    private void OnDestroy()
    {
        ENet.Library.Deinitialize();
    }

    protected void AddManager<T>() where T : Singleton
    {
        var manager = new GameObject().AddComponent<T>();
        manager.name = typeof(T).Name;
        manager.transform.parent = transform;

        managers.Add(manager);
    }
}
