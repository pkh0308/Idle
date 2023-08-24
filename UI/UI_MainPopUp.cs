using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainPopUp : UI_PopUp
{
    enum Texts
    {
        StageText,
        EnhanceBtnText,
        WeaponBtnText,
        TreasureBtnText,
        ShopBtnText
    }
    enum Buttons
    {
        EnhanceBtn,
        WeaponBtn,
        TreasureBtn,
        ShopBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.EnhanceBtn).gameObject).BindEvent(Btn_OnClickEnhance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.WeaponBtn).gameObject).BindEvent(Btn_OnClickWeapon);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TreasureBtn).gameObject).BindEvent(Btn_OnClickTreasure);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ShopBtn).gameObject).BindEvent(Btn_OnClickShop);

        Managers.UI.OpenPopUp<UI_EnhancePopUp>(typeof(UI_EnhancePopUp).Name, transform);
        return true;
    }

    #region 버튼
    public void Btn_OnClickEnhance()
    {
        Debug.Log("강화");
    }
    public void Btn_OnClickWeapon()
    {
        Debug.Log("무기");
    }
    public void Btn_OnClickTreasure()
    {
        Debug.Log("보물");
    }
    public void Btn_OnClickShop()
    {
        Debug.Log("상점");
    }
    #endregion
}
