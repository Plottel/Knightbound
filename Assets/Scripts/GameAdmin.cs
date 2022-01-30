using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;


public class GameAdmin : MonoBehaviour
{
    private List<Singleton> managers;

    void Awake()
    {
        managers = new List<Singleton>();
    }

    void Start()        => NotifyManagersStart();
    void Update()       => NotifyManagersUpdate();
    void LateUpdate()   => NotifyManagersLateUpdate();

    public void NotifyManagersAwake()
    {
        foreach (var manager in managers)
            manager.OnAwake();
    }

    public void NotifyManagersStart()
    {
        foreach (var manager in managers)
            manager.OnStart();
    }

    public void NotifyManagersUpdate()
    {
        foreach (var manager in managers)
            manager.OnUpdate();
    }

    public void NotifyManagersLateUpdate()
    {
        foreach (var manager in managers)
            manager.OnLateUpdate();
    }

    public void AddManager<T>() where T : Singleton
    {
        var manager = new GameObject().AddComponent<T>();
        manager.name = typeof(T).Name;
        manager.transform.parent = transform;
        manager.EnsureInstance();

        managers.Add(manager);
    }
}
