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

    #region πˆ∆∞
    // ∞ÒµÂ
    const int Gold_2Hour = 2;
    const int Gold_5Hour = 5;
    // ∫∏ºÆ
    const int Gem_100 = 100;
    const int Gem_500 = 500;
    const int Gem_2500 = 2500;

    public void Btn_OnClickGetGold_2hr()
    {
        if(Managers.Adv.CanShowAd((int)Buttons.GetGoldBtn_2hr, ConstValue.Ads.Gold_2hr) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_AdCountOver);
            return;
        }

        Managers.Adv.ShowRewardedAds((rwd) =>
        {
            Managers.Game.GetGoldPerHour(Gold_2Hour);
            Managers.UI.OpenNotice(ConstValue.Notice_AdReward);
            UpdateBtns();
        });
    }
    public void Btn_OnClickGetGold_5hr()
    {
        Debug.Log("∞ÒµÂ »πµÊ(5Ω√∞£)");
    }
    public void Btn_OnClickGetGem_100()
    {
        if (Managers.Adv.CanShowAd((int)Buttons.GetGemBtn_100, ConstValue.Ads.Gem_100) == false)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_AdCountOver);
            return;
        }

        Managers.Adv.ShowRewardedAds((rwd) =>
        {
            Managers.Game.GetGem(Gem_100);
            Managers.UI.OpenNotice(ConstValue.Notice_AdReward);
            UpdateBtns();
        });
    }
    public void Btn_OnClickGetGem_500()
    {
        Debug.Log("¡™ »πµÊ(500)");
    }
    public void Btn_OnClickGetGem_2500()
    {
        Debug.Log("¡™ »πµÊ(2500)");
    }
    #endregion

    #region UI æ˜µ•¿Ã∆Æ
    void UpdateBtns()
    {
        int gold_2hr = (int)Buttons.GetGoldBtn_2hr;
        int gold_5hr = (int)Buttons.GetGoldBtn_5hr;
        int gem_100 = (int)Buttons.GetGemBtn_100;
        int gem_500 = (int)Buttons.GetGemBtn_500;
        int gem_2500 = (int)Buttons.GetGemBtn_2500;

        _getGoldBtnText_2hr.text = GetCost(Managers.Data.GetShopData(gold_2hr), (int)ConstValue.Ads.Gold_2hr);
        _getGoldBtnText_5hr.text = GetCost(Managers.Data.GetShopData(gold_5hr));
        _getGemBtnText_100.text = GetCost(Managers.Data.GetShopData(gem_100), (int)ConstValue.Ads.Gem_100);
        _getGemBtnText_500.text = GetCost(Managers.Data.GetShopData(gem_500));
        _getGemBtnText_2500.text = GetCost(Managers.Data.GetShopData(gem_2500));
    }

    string GetCost(ShopData data, int adIdx = -1)
    {
        if(adIdx >= 0) // ±§∞Ì ªÛ«∞
            return $"({Managers.Game.GetAdCount(adIdx)} / {data.MaxCount})";
            
        return "£‹ " + data.Cost.ToString();
    }
    #endregion
}