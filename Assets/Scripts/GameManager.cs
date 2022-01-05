using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;
using Deft.Networking;
using ParrelSync;

public class GameManager<T_DerivedGM> : Manager<T_DerivedGM> where T_DerivedGM : Singleton
{
    private List<Singleton> managers;

    void Awake()
    {
        managers = new List<Singleton>();

        OnAwake();
        NotifyManagersAwake();
    }

    void Start()
    {
        OnStart();
        NotifyManagersStart();
    }

    void Update()
    {
        OnUpdate();
        NotifyManagersUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
        NotifyManagersLateUpdate();
    }

    protected void NotifyManagersAwake()
    {
        foreach (var manager in managers)
            manager.OnAwake();
    }

    protected void NotifyManagersStart()
    {
        foreach (var manager in managers)
            manager.OnStart();
    }

    protected void NotifyManagersUpdate()
    {
        foreach (var manager in managers)
            manager.OnUpdate();
    }

    protected void NotifyManagersLateUpdate()
    {
        foreach (var manager in managers)
            manager.OnLateUpdate();
    }    

    protected void AddManager<T>() where T : Singleton
    {
        var manager = new GameObject().AddComponent<T>();
        manager.name = typeof(T).Name;
        manager.transform.parent = transform;

        managers.Add(manager);
    }
}
