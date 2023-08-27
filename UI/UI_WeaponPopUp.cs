using System.Collections;
using System.Collections.Generic;
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

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.HammerBuyBtn).gameObject).BindEvent(Btn_OnClickHammerBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.SpearBuyBtn).gameObject).BindEvent(Btn_OnClickSpearBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.AxeBuyBtn).gameObject).BindEvent(Btn_OnClickAxeBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.SwordBuyBtn).gameObject).BindEvent(Btn_OnClickSwordBuy);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TwinSwordsBuyBtn).gameObject).BindEvent(Btn_OnClickTwinSwordsBuy);

        return true;
    }

    #region 버튼
    public void Btn_OnClickHammerBuy()
    {
        Debug.Log("망치 구매");
    }
    public void Btn_OnClickSpearBuy()
    {
        Debug.Log("창 구매");
    }
    public void Btn_OnClickAxeBuy()
    {
        Debug.Log("도끼 구매");
    }
    public void Btn_OnClickSwordBuy()
    {
        Debug.Log("검 구매");
    }
    public void Btn_OnClickTwinSwordsBuy()
    {
        Debug.Log("쌍검 구매");
    }
    #endregion
}
