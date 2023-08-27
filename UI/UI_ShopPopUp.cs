using System.Collections;
using System.Collections.Generic;
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

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGoldBtn_2hr).gameObject).BindEvent(Btn_OnClickGetGold_2hr);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGoldBtn_5hr).gameObject).BindEvent(Btn_OnClickGetGold_5hr);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_100).gameObject).BindEvent(Btn_OnClickGetGem_100);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_500).gameObject).BindEvent(Btn_OnClickGetGem_500);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.GetGemBtn_2500).gameObject).BindEvent(Btn_OnClickGetGem_2500);

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
}