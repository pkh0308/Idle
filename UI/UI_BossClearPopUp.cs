using TMPro;

public class UI_BossClearPopUp : UI_PopUp
{
    #region ���� �� ������
    enum Texts
    {
        AnnounceText,
        ExitBtnText
    }
    TextMeshProUGUI _announceText;
    const string BossClear = "���� ����� �����Ͽ����ϴ�!";
    const string TimeOut = "���� ����� �����߽��ϴ�...";

    enum Buttons
    {
        ExitBtn
    }
    #endregion

    #region �ʱ�ȭ
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

    #region ��ư
    public void Btn_OnClickExit()
    {
        Managers.Game.ExitBossTry();
    }
    #endregion
}
