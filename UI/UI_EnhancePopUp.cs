using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnhancePopUp : UI_PopUp
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

        Managers.UI.OpenPopUp<UI_EnhancePopUp>();
        return true;
    }

    #region ��ư
    public void Btn_OnClickAtkPower()
    {
        Debug.Log("���ݷ� ����");
    }
    public void Btn_OnClickAtkSpeed()
    {
        Debug.Log("���ݼӵ� ����");
    }
    public void Btn_OnClickCritChance()
    {
        Debug.Log("ġ��Ÿ Ȯ�� ����");
    }
    public void Btn_OnClickCritDamage()
    {
        Debug.Log("ġ��Ÿ ������ ����");
    }
    public void Btn_OnClickGoldUp()
    {
        Debug.Log("ȹ�� ��� ����");
    }
    #endregion
}
