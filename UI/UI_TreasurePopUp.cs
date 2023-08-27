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

    #region 버튼
    public void Btn_OnClickAtkPower()
    {
        Debug.Log("싸움꾼의 장갑 구매");
    }
    public void Btn_OnClickAtkSpeed()
    {
        Debug.Log("신속의 깃털 구매");
    }
    public void Btn_OnClickCritChance()
    {
        Debug.Log("행운의 반지 구매");
    }
    public void Btn_OnClickCritDamage()
    {
        Debug.Log("바이킹의 투구 구매");
    }
    public void Btn_OnClickGoldUp()
    {
        Debug.Log("부자의 왕관 구매");
    }
    #endregion
}
