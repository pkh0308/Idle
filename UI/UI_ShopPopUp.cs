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
    public void Btn_OnClickGetGold_2hr()
    {
        Debug.Log("∞ÒµÂ »πµÊ(2Ω√∞£)");
    }
    public void Btn_OnClickGetGold_5hr()
    {
        Debug.Log("∞ÒµÂ »πµÊ(5Ω√∞£)");
    }
    public void Btn_OnClickGetGem_100()
    {
        Debug.Log("¡™ »πµÊ(100)");
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

        _getGoldBtnText_2hr.text = GetCost(Managers.Data.GetShopData(gold_2hr));
        _getGoldBtnText_5hr.text = GetCost(Managers.Data.GetShopData(gold_5hr));
        _getGemBtnText_100.text = GetCost(Managers.Data.GetShopData(gem_100));
        _getGemBtnText_500.text = GetCost(Managers.Data.GetShopData(gem_500));
        _getGemBtnText_2500.text = GetCost(Managers.Data.GetShopData(gem_2500));
    }

    string GetCost(ShopData data)
    {
        if(data.MaxCount > 0) // ±§∞Ì ªÛ«∞
            return $"({0} / {data.MaxCount})";
        
        return "£‹ " + data.Cost.ToString();
    }
    #endregion
}