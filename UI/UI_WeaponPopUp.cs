using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WeaponPopUp : UI_PopUp
{
    enum Texts
    {
        HammerText,
        HammerPriceText,
        SpearText,
        SpearPriceText,
        AxeText,
        AxePriceText,
        SwordText,
        SwordPriceText,
        TwinSwordsText,
        TwinSwordsPriceText
    }
    TextMeshProUGUI _hammerPriceText;
    TextMeshProUGUI _spearPriceText;
    TextMeshProUGUI _axePriceText;
    TextMeshProUGUI _swordPriceText;
    TextMeshProUGUI _twinSwordsPriceText;

    enum Buttons
    {
        HammerBuyBtn,
        SpearBuyBtn,
        AxeBuyBtn,
        SwordBuyBtn,
        TwinSwordsBuyBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _hammerPriceText = GetText((int)Texts.HammerPriceText);
        _spearPriceText = GetText((int)Texts.SpearPriceText);
        _axePriceText = GetText((int)Texts.AxePriceText);
        _swordPriceText = GetText((int)Texts.SwordPriceText);
        _twinSwordsPriceText = GetText((int)Texts.TwinSwordsPriceText);

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.HammerBuyBtn).gameObject).BindEvent(Btn_OnClickHammerBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.SpearBuyBtn).gameObject).BindEvent(Btn_OnClickSpearBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AxeBuyBtn).gameObject).BindEvent(Btn_OnClickAxeBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.SwordBuyBtn).gameObject).BindEvent(Btn_OnClickSwordBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TwinSwordsBuyBtn).gameObject).BindEvent(Btn_OnClickTwinSwordsBuy);

        UpdateBtns();
        return true;
    }

    #region 버튼
    public void Btn_OnClickHammerBuy()
    {
        int level = (int)Buttons.HammerBuyBtn;
        if (Managers.Game.WeaponLv > level)
            return;
        
        if(Managers.Game.BuyWeapon(level) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickSpearBuy()
    {
        int level = (int)Buttons.SpearBuyBtn;
        if (Managers.Game.WeaponLv > level)
            return;

        if (Managers.Game.BuyWeapon(level) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickAxeBuy()
    {
        int level = (int)Buttons.AxeBuyBtn;
        if (Managers.Game.WeaponLv > level)
            return;

        if (Managers.Game.BuyWeapon(level) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickSwordBuy()
    {
        int level = (int)Buttons.SwordBuyBtn;
        if (Managers.Game.WeaponLv > level)
            return;

        if (Managers.Game.BuyWeapon(level) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    public void Btn_OnClickTwinSwordsBuy()
    {
        int level = (int)Buttons.TwinSwordsBuyBtn;
        if (Managers.Game.WeaponLv > level)
            return;

        if (Managers.Game.BuyWeapon(level) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotEnoughMoney);
            return;
        }

        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Upgrade);
        UpdateBtns();
    }
    #endregion

    #region UI 갱신
    void UpdateBtns()
    {
        int curLv = Managers.Game.WeaponLv;
        int hammerLv = (int)Buttons.HammerBuyBtn;
        int spearLv = (int)Buttons.SpearBuyBtn;
        int axeLv = (int)Buttons.AxeBuyBtn;
        int swordLv = (int)Buttons.SwordBuyBtn;
        int twinLv = (int)Buttons.TwinSwordsBuyBtn;

        _hammerPriceText.text = curLv > hammerLv ? ConstValue.Max : Custom.CalUnit(Managers.Data.GetWeaponData(hammerLv).Cost);
        _spearPriceText.text = curLv > spearLv ? ConstValue.Max : Custom.CalUnit(Managers.Data.GetWeaponData(spearLv).Cost);
        _axePriceText.text = curLv > axeLv ? ConstValue.Max : Custom.CalUnit(Managers.Data.GetWeaponData(axeLv).Cost);
        _swordPriceText.text = curLv > swordLv ? ConstValue.Max : Custom.CalUnit(Managers.Data.GetWeaponData(swordLv).Cost);
        _twinSwordsPriceText.text = curLv > twinLv ? ConstValue.Max : Custom.CalUnit(Managers.Data.GetWeaponData(twinLv).Cost);
    }
    #endregion
}
