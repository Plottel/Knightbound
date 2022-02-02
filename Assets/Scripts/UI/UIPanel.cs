using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public delegate void VisibilityChangedHandler(bool visible);
    public VisibilityChangedHandler eventVisibilityChanged;

    [HideInInspector]
    public Canvas canvas;

    private bool isVisible = true;
    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (isVisible != value)
            {
                isVisible = value;
                canvas.gameObject.SetActive(value);
                eventVisibilityChanged?.Invoke(value);
            }
        }

    }

    void Awake()
    {
        eventVisibilityChanged += OnVisibilityChanged;
        OnAwake();
    }

    void Start() => OnStart();

    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnVisibilityChanged(bool value) { }
}
