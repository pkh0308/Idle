using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : SceneController
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _initialized = true;
        Managers.UI.OpenPopUp<UI_MainPopUp>();
        return true;
    }
}
