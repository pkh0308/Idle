using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BossExitPopUp : UI_PopUp
{
    #region 변수 및 열거형
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
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitOkBtn).gameObject).BindEvent(Btn_OnClickExitOk);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitCancelBtn).gameObject).BindEvent(Btn_OnClickExitCancel);

        return true;
    }
    #endregion

    #region 버튼
    public void Btn_OnClickExitOk()
    {
        Managers.Game.ExitBossTry();
    }

    public void Btn_OnClickExitCancel()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion


}