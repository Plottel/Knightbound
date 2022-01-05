using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParrelSync;
using Deft;

public class ProgramEntryPoint : MonoBehaviour
{
    public GameResources GameResources;

    void Awake()
    {
        // Setup Game Resources
        GameResources.OnAwake();

        if (ClonesManager.IsClone())
            InitializeClient();
        else
        {
            InitializeServer();
            InitializeClient();
        }
    }

    private void Start()
    {
        Destroy(gameObject);
    }

    void InitializeClient()
    {
        SpawnManager<GameManagerClient>();
    }

    void InitializeServer()
    {
        SpawnManager<GameManagerServer>();
    }

    protected T SpawnManager<T>() where T : Singleton
    {
        var manager = new GameObject().AddComponent<T>();
        manager.name = typeof(T).Name;

        return manager;
    }
}
