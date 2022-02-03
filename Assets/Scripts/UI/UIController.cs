using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class UIController : Manager<UIController>
{
    public override void OnAwake()
    {
        GameStateManager.Get.eventEnterGameState += OnEnterGameState;
        GameStateManager.Get.eventExitGameState += OnExitGameState;

        UIView.Get.GenerateUI(GameResources.Get.CanvasTemplate, GameResources.Get.GamePanels);
    }

    private void OnEnterGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu: 
                UIView.Get.GetPanel<MainMenu>().IsVisible = true; 
                break;
        }
    }

    private void OnExitGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                UIView.Get.GetPanel<MainMenu>().IsVisible = false;
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Get.eventEnterGameState -= OnEnterGameState;
        GameStateManager.Get.eventExitGameState -= OnExitGameState;

    }
}
