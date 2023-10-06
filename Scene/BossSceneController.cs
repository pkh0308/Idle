using UnityEngine;

public class BossSceneController : SceneController
{
    GameObject _loadingScreen;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _initialized = true;
        _loadingScreen = GameObject.Find(ConstValue.LoadingScreen);

        Managers.UI.OpenScene<UI_BossPopUp>(() =>
        {
            Destroy(_loadingScreen);
        });
        return true;
    }
}