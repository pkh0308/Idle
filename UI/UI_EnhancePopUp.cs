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

        _atkPowerBtnText = GetText((int)Texts.AtkPowerBtnText);
        _atkSpeedBtnText = GetText((int)Texts.AtkSpeedBtnText);
        _critChanceBtnText = GetText((int)Texts.CritChanceBtnText);
        _critDamageBtnText = GetText((int)Texts.CritDamageBtnText);
        _goldUpBtnText = GetText((int)Texts.GoldUpBtnText);

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AtkPowerBtn).gameObject).BindEvent(Btn_OnClickAtkPower);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AtkSpeedBtn).gameObject).BindEvent(Btn_OnClickAtkSpeed);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CritChanceBtn).gameObject).BindEvent(Btn_OnClickCritChance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CritDamageBtn).gameObject).BindEvent(Btn_OnClickCritDamage);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GoldUpBtn).gameObject).BindEvent(Btn_OnClickGoldUp);

        UpdateBtns();
        return true;
    }

    #region 버튼
    public void Btn_OnClickAtkPower()
    {
        if(Managers.Game.EnhanceAtkPow() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickAtkSpeed()
    {
        if (Managers.Game.EnhanceAtkSpd() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickCritChance()
    {
        if (Managers.Game.EnhanceCritChance() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickCritDamage()
    {
        if (Managers.Game.EnhanceCritDamage() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickGoldUp()
    {
        if (Managers.Game.EnhanceGoldUp() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney, transform);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    #endregion

    #region UI 업데이트
    void UpdateBtns()
    {
        int atkPow = (int)Buttons.AtkPowerBtn;
        int atkSpd = (int)Buttons.AtkSpeedBtn;
        int critChance = (int)Buttons.CritChanceBtn;
        int critDmg = (int)Buttons.CritDamageBtn;
        int goldUp = (int)Buttons.GoldUpBtn;

        _atkPowerBtnText.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(atkPow, Managers.Game.AtkPowerLv));
        _atkSpeedBtnText.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(atkSpd, Managers.Game.AtkPowerLv));
        _critChanceBtnText.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(critChance, Managers.Game.AtkPowerLv));
        _critDamageBtnText.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(critDmg, Managers.Game.AtkPowerLv));
        _goldUpBtnText.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(goldUp, Managers.Game.AtkPowerLv));
    }
    #endregion
}