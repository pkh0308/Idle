using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitPopUp : UI_PopUp
{
    enum Texts
    {
        ExitText,
        ExitOkBtnText,
        ExitCancelBtnText
    }
    enum Buttons
    {
        ExitOkBtn,
        ExitCancelBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitOkBtn).gameObject).BindEvent(Btn_OnClickExitOk);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitCancelBtn).gameObject).BindEvent(Btn_OnClickExitCancle);

        return true;
    }

    #region 버튼 콜백
    public void Btn_OnClickExitOk()
    {
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_BtnClick);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void Btn_OnClickExitCancle()
    {
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_BtnClick);
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}
