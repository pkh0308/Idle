using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TextMeshProUGUI _atkPowerBtnText;
    TextMeshProUGUI _atkSpeedBtnText;
    TextMeshProUGUI _critChanceBtnText;
    TextMeshProUGUI _critDamageBtnText;
    TextMeshProUGUI _goldUpBtnText;

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
        Debug.Log("공격력 증가");

        if(Managers.Game.EnhanceAtkPow() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);

        UpdateEnhanceBtns();
    }
    public void Btn_OnClickAtkSpeed()
    {
        Debug.Log("공격속도 증가");

        if (Managers.Game.EnhanceAtkSpd() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);

        UpdateEnhanceBtns();
    }
    public void Btn_OnClickCritChance()
    {
        Debug.Log("치명타 확률 증가");

        if (Managers.Game.EnhanceCritChance() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);

        UpdateEnhanceBtns();
    }
    public void Btn_OnClickCritDamage()
    {
        Debug.Log("치명타 데미지 증가");

        if (Managers.Game.EnhanceCritDamage() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);

        UpdateEnhanceBtns();
    }
    public void Btn_OnClickGoldUp()
    {
        Debug.Log("획득 골드 증가");

        if (Managers.Game.EnhanceGoldUp() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);

        UpdateEnhanceBtns();
    }
    #endregion

    #region UI 업데이트
    void UpdateEnhanceBtns()
    {
        _atkPowerBtnText.text = Managers.Data.GetEnahnceCost(0, Managers.Game.AtkPowerLv).ToString();
        _atkSpeedBtnText.text = Managers.Data.GetEnahnceCost(0, Managers.Game.AtkSpeedLv).ToString();
        _critChanceBtnText.text = Managers.Data.GetEnahnceCost(0, Managers.Game.CritChanceLv).ToString();
        _critDamageBtnText.text = Managers.Data.GetEnahnceCost(0, Managers.Game.CritDamageLv).ToString();
        _goldUpBtnText.text = Managers.Data.GetEnahnceCost(0, Managers.Game.GoldUpLv).ToString();
    }
    #endregion
}
