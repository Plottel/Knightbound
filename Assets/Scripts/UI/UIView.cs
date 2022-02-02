using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class UIView : Manager<UIView>
{
    Dictionary<Type, UIPanel> panels;

    public override void OnAwake()
    {
        panels = new Dictionary<Type, UIPanel>();
    }

    public T GetPanel<T>() where T : UIPanel
        => panels[typeof(T)] as T;

    public void GenerateUI(Canvas canvasTemplate, UIPanelCache panelCache)
    {
        panels.Clear();

        // Spawn Panels
        foreach (UIPanel panelPrefab in panelCache.prefabs)
        {
            Type panelType = panelPrefab.GetType();
            UIPanel panel = GeneratePanel(canvasTemplate, panelPrefab);

            panels.Add(panelType, panel);
        }
    }

    UIPanel GeneratePanel(Canvas canvasTemplate, UIPanel panelPrefab)
    {
        Canvas newCanvas = Instantiate(canvasTemplate, transform);
        UIPanel newPanel = Instantiate(panelPrefab, newCanvas.transform);

        newPanel.canvas = newCanvas;
        newPanel.IsVisible = false;
        newPanel.name = newPanel.name.Replace("(Clone)", string.Empty);
        newCanvas.name = string.Concat("Canvas-", newPanel.name);

        return newPanel;
    }
}
