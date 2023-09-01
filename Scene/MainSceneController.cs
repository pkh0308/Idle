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
        Managers.UI.OpenScene<UI_MainPopUp>();
        return true;
    }
}