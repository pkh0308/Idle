using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossClearPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        ClearText,
        ExitBtnText
    }

    enum Buttons
    {
        ExitBtn
    }
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }
    #endregion

    #region 버튼
    public void Btn_OnClickExit()
    {
        Managers.Game.ExitBossTry();
    }
    #endregion
}
