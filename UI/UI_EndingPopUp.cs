// ������ UI
public class UI_EndingPopUp : UI_PopUp
{
    enum Texts
    {
        CongratulationText,
        ClearText,
        ExitBtnText
    }

    enum Buttons
    {
        ExitBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_BossClear);

        return true;
    }

    #region ��ư
    public void Btn_OnClickExit()
    {
        Managers.Game.BackToTitle();
    }
    #endregion
}