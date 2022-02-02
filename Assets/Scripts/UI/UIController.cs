using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class UIController : Manager<UIController>
{
    public override void OnAwake()
    {
        UIView.Get.GenerateUI(GameResources.Get.CanvasTemplate, GameResources.Get.GamePanels);
        UIView.Get.GetPanel<MainMenu>().IsVisible = true;
    }
}
