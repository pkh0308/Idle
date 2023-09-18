using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TextMeshProUGUI _atkPowerText;
    TextMeshProUGUI _atkSpeedText;
    TextMeshProUGUI _critChanceText;
    TextMeshProUGUI _critDamageText;
    TextMeshProUGUI _goldUpText;
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

        _atkPowerText = GetText((int)Texts.AtkPowerText);
        _atkSpeedText = GetText((int)Texts.AtkSpeedText);
        _critChanceText = GetText((int)Texts.CritChanceText);
        _critDamageText = GetText((int)Texts.CritDamageText);
        _goldUpText = GetText((int)Texts.GoldUpText);
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
        if (Managers.Game.BuyTreasure_AtkPow() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickAtkSpeed()
    {
        if (Managers.Game.BuyTreasure_AtkSpd() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickCritChance()
    {
        if (Managers.Game.BuyTreasure_CritChance() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickCritDamage()
    {
        if (Managers.Game.BuyTreasure_CritDmg() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickGoldUp()
    {
        if (Managers.Game.BuyTreasure_GoldUp() == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
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

        // 설명 업데이트
        _atkPowerText.text = $"싸움꾼의 장갑 (Lv.{Managers.Data.GetTrLevelByStr(Managers.Game.Tr_AtkPowerLv)})\n- 공격력 {Managers.Data.GetTrValueByStr(Managers.Game.Tr_AtkPowerLv, atkPow)} 증가";
        _atkSpeedText.text = $"신속의 깃털 Lv.{Managers.Data.GetTrLevelByStr(Managers.Game.Tr_AtkSpeedLv)}\n- 공격속도 {Managers.Data.GetTrValueByStr(Managers.Game.Tr_AtkSpeedLv, atkSpd)} 증가";
        _critChanceText.text = $"행운의 반지 Lv.{Managers.Data.GetTrLevelByStr(Managers.Game.Tr_CritChanceLv)}\n- 치명타 확률 {Managers.Data.GetTrValueByStr(Managers.Game.Tr_CritChanceLv, critChance)} 증가";
        _critDamageText.text = $"바이킹의 투구 Lv.{Managers.Data.GetTrLevelByStr(Managers.Game.Tr_CritDamageLv)}\n- 치명타 데미지 {Managers.Data.GetTrValueByStr(Managers.Game.Tr_CritDamageLv, critDmg)} 증가";
        _goldUpText.text = $"부자의 왕관 Lv.{Managers.Data.GetTrLevelByStr(Managers.Game.Tr_GoldUpLv)}\n- 획득 골드 {Managers.Data.GetTrValueByStr(Managers.Game.Tr_GoldUpLv, goldUp)} 증가";

        // 가격 업데이트
        _atkPowerBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(atkPow, Managers.Game.Tr_AtkPowerLv));
        _atkSpeedBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(atkSpd, Managers.Game.Tr_AtkSpeedLv));
        _critChanceBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(critChance, Managers.Game.Tr_CritChanceLv));
        _critDamageBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(critDmg, Managers.Game.Tr_CritDamageLv));
        _goldUpBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(goldUp, Managers.Game.Tr_GoldUpLv));
    }
    #endregion
}
