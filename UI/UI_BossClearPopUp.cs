using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossClearPopUp : UI_PopUp
{
    #region ���� �� ������
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

    #region �ʱ�ȭ
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }
    #endregion

    #region ��ư
    public void Btn_OnClickExit()
    {
        Managers.Game.ExitBossTry();
    }
    #endregion
}
