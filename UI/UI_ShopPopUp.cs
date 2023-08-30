using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShopPopUp : UI_PopUp
{
    enum Texts
    {
        GoldText_2hr,
        GetGoldBtnText_2hr,
        GoldText_5hr,
        GetGoldBtnText_5hr,
        GemText_100,
        GetGemBtnText_100,
        GemText_500,
        GetGemBtnText_500,
        GemText_2500,
        GetGemBtnText_2500
    }
    TextMeshProUGUI _getGoldBtnText_2hr;
    TextMeshProUGUI _getGoldBtnText_5hr;
    TextMeshProUGUI _getGemBtnText_100;
    TextMeshProUGUI _getGemBtnText_500;
    TextMeshProUGUI _getGemBtnText_2500;

    enum Buttons
    {
        GetGoldBtn_2hr,
        GetGoldBtn_5hr,
        GetGemBtn_100,
        GetGemBtn_500,
        GetGemBtn_2500
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _getGoldBtnText_2hr = GetText((int)Texts.GetGoldBtnText_2hr);
        _getGoldBtnText_5hr = GetText((int)Texts.GetGoldBtnText_5hr);
        _getGemBtnText_100 = GetText((int)Texts.GetGemBtnText_100);
        _getGemBtnText_500 = GetText((int)Texts.GetGemBtnText_500);
        _getGemBtnText_2500 = GetText((int)Texts.GetGemBtnText_2500);

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGoldBtn_2hr).gameObject).BindEvent(Btn_OnClickGetGold_2hr);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGoldBtn_5hr).gameObject).BindEvent(Btn_OnClickGetGold_5hr);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_100).gameObject).BindEvent(Btn_OnClickGetGem_100);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_500).gameObject).BindEvent(Btn_OnClickGetGem_500);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_2500).gameObject).BindEvent(Btn_OnClickGetGem_2500);

        UpdateBtns();
        return true;
    }

    #region ¹öÆ°
    public void Btn_OnClickGetGold_2hr()
    {
        Debug.Log("°ñµå È¹µæ(2½Ã°£)");
    }
    public void Btn_OnClickGetGold_5hr()
    {
        Debug.Log("°ñµå È¹µæ(5½Ã°£)");
    }
    public void Btn_OnClickGetGem_100()
    {
        Debug.Log("Áª È¹µæ(100)");
    }
    public void Btn_OnClickGetGem_500()
    {
        Debug.Log("Áª È¹µæ(500)");
    }
    public void Btn_OnClickGetGem_2500()
    {
        Debug.Log("Áª È¹µæ(2500)");
    }
    #endregion

    #region UI ¾÷µ¥ÀÌÆ®
    void UpdateBtns()
    {
        int atkPow = (int)Buttons.GetGoldBtn_2hr;
        int atkSpd = (int)Buttons.GetGoldBtn_5hr;
        int critChance = (int)Buttons.GetGemBtn_100;
        int critDmg = (int)Buttons.GetGemBtn_500;
        int goldUp = (int)Buttons.GetGemBtn_2500;

        //_getGoldBtnText_2hr.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(atkPow, Managers.Game.AtkPowerLv));
        //_getGoldBtnText_5hr.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(atkSpd, Managers.Game.AtkPowerLv));
        //_getGemBtnText_100.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(critChance, Managers.Game.AtkPowerLv));
        //_getGemBtnText_500.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(critDmg, Managers.Game.AtkPowerLv));
        //_getGemBtnText_2500.text = Custom.CalUnit(Managers.Data.GetEnahnceCost(goldUp, Managers.Game.AtkPowerLv));
    }
    #endregion
}