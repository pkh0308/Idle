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

    #region ��ư
    public void Btn_OnClickAtkPower()
    {
        Debug.Log("�ο���� �尩 ����");
    }
    public void Btn_OnClickAtkSpeed()
    {
        Debug.Log("�ż��� ���� ����");
    }
    public void Btn_OnClickCritChance()
    {
        Debug.Log("����� ���� ����");
    }
    public void Btn_OnClickCritDamage()
    {
        Debug.Log("����ŷ�� ���� ����");
    }
    public void Btn_OnClickGoldUp()
    {
        Debug.Log("������ �հ� ����");
    }
    #endregion

    #region UI ������Ʈ
    void UpdateBtns()
    {
        int atkPow = (int)Buttons.AtkPowerBtn;
        int atkSpd = (int)Buttons.AtkSpeedBtn;
        int critChance = (int)Buttons.CritChanceBtn;
        int critDmg = (int)Buttons.CritDamageBtn;
        int goldUp = (int)Buttons.GoldUpBtn;

        _atkPowerBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(atkPow, Managers.Game.AtkPowerLv));
        _atkSpeedBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(atkSpd, Managers.Game.AtkPowerLv));
        _critChanceBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(critChance, Managers.Game.AtkPowerLv));
        _critDamageBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(critDmg, Managers.Game.AtkPowerLv));
        _goldUpBtnText.text = Custom.CalUnit(Managers.Data.GetTreasureCost(goldUp, Managers.Game.AtkPowerLv));
    }
    #endregion
}
