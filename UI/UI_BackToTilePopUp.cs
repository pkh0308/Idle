

public class UI_BackToTilePopUp : UI_PopUp
{
    enum Texts
    {
        BackToTitleText,
        BackToTitleBtnText,
        CancelBtnText
    }
    enum Buttons
    {
        BackToTitleBtn,
        CancleBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.BackToTitleBtn).gameObject).BindEvent(Btn_OnClickBackToTitle);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CancleBtn).gameObject).BindEvent(Btn_OnClickCancle);

        return true;
    }

    #region ¹öÆ°
    public void Btn_OnClickBackToTitle()
    {
        Managers.Game.BackToTitle();
    }

    public void Btn_OnClickCancle()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}