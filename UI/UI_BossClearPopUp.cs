using TMPro;

public class UI_BossClearPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        AnnounceText,
        ExitBtnText
    }
    TextMeshProUGUI _announceText;
    const string BossClear = "보스 토벌에 성공하였습니다!";
    const string TimeOut = "보스 토벌에 실패했습니다...";

    enum Buttons
    {
        ExitBtn
    }
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _announceText = GetText((int)Texts.AnnounceText);
        _announceText.text = Managers.Game.EnemyHp > 0 ? TimeOut : BossClear; 

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }
    #endregion

    #region 버튼
    public void Btn_OnClickExit()
    {
        Managers.Game.ExitBossTry();
    }
    #endregion
}
