using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : SceneController
{
    GameObject _loadingScreen;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _initialized = true;
        _loadingScreen = GameObject.Find(ConstValue.LoadingScreen);

        Managers.UI.OpenScene<UI_TitlePopUp>(() =>
        {
            Destroy(_loadingScreen);
        });
        return true;
    }
}
