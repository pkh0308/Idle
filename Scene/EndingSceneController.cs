using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneController : SceneController
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _initialized = true;
        Managers.UI.OpenScene<UI_EndingPopUp>();
        return true;
    }
}