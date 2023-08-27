using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TreasurePopUp : UI_PopUp
{
    enum Texts
    {
        AtkPowerText,
        AtkPowerBtnText,
        AtkSpeedText,
        AtkSpeedBtnText,
        CritChanceText,
        CritChanceBtnText,
        CritDamageText,
        CritDamageBtnText,
        GoldUpText,
        GoldUpBtnText
    }
    enum Buttons
    {
        AtkPowerBtn,
        AtkSpeedBtn,
        CritChanceBtn,
        CritDamageBtn,
        GoldUpBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AtkPowerBtn).gameObject).BindEvent(Btn_OnClickAtkPower);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AtkSpeedBtn).gameObject).BindEvent(Btn_OnClickAtkSpeed);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CritChanceBtn).gameObject).BindEvent(Btn_OnClickCritChance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CritDamageBtn).gameObject).BindEvent(Btn_OnClickCritDamage);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GoldUpBtn).gameObject).BindEvent(Btn_OnClickGoldUp);

        return true;
    }

    #region ��ư
    public void Btn_OnClickAtkPower()
    {
        Debug.Log("�ο���� �尩 ����");
    }
    public void Btn_OnClickAtkSpeed()
    {
        Debug.Log("�ż��� ���� ����");
    }
    public void Btn_OnClickCritChance()
    {
        Debug.Log("����� ���� ����");
    }
    public void Btn_OnClickCritDamage()
    {
        Debug.Log("����ŷ�� ���� ����");
    }
    public void Btn_OnClickGoldUp()
    {
        Debug.Log("������ �հ� ����");
    }
    #endregion
}
