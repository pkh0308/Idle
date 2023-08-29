using TMPro;
using UnityEngine;

public class UI_NewStartPopUp : UI_PopUp
{
    enum Texts
    {
        IDInputText,
        IDText,
        NewStartBtnText,
        ExitBtnText
    }
    enum Buttons
    {
        NewStartBtn,
        ExitBtn
    }

    TextMeshProUGUI idText;

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        idText = GetText((int)Texts.IDText);

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.NewStartBtn).gameObject).BindEvent(Btn_OnClickNewStart);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }

    #region ¹öÆ°
    public void Btn_OnClickNewStart()
    {
        if(idText.text.Length < 2 + 1)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_LessThan2, transform);
            idText.text = "";
            return;
        }

        string replaced = idText.text.Replace(" ", "");
        if(idText.text.Length != replaced.Length)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NoBlankInName, transform);
            idText.text = "";
            return;
        }

        Managers.Game.StartNewGame(idText.text);
    }

    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}
