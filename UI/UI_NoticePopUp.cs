using TMPro;

public class UI_NoticePopUp : UI_PopUp
{
    enum Texts
    {
        NoticeText,
        NoticeExitBtnText
    }
    enum Buttons
    {
        NoticeExitBtn
    }
    TextMeshProUGUI noticeText;

    public override bool Init()
    {
        if(_initialized) 
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        noticeText = GetText((int)Texts.NoticeText);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.NoticeExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        _initialized = true;
        return true;
    }

    public void SetNoticeText(string notice)
    {
        noticeText.text = notice;
    }

    #region ¹öÆ°
    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}
