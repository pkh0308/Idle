using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UI_BossTryPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        Boss_01BtnText,
        Boss_02BtnText,
        Boss_03BtnText,
        Boss_04BtnText,
        Boss_05BtnText,
        TitleText,
        DescriptionText,
        ExitBtnText
    }
    List<TextMeshProUGUI> _bossBtnTexts = new List<TextMeshProUGUI>(BossCount);

    enum Buttons
    {
        Boss_01Btn,
        Boss_02Btn, 
        Boss_03Btn,
        Boss_04Btn,
        Boss_05Btn,
        BossTryBtn,
        ExitBtn
    }
    enum Images
    {
        Boss_01Btn_Lock,
        Boss_02Btn_Lock,
        Boss_03Btn_Lock,
        Boss_04Btn_Lock,
        Boss_05Btn_Lock,
        Boss_01Btn_Cleared,
        Boss_02Btn_Cleared,
        Boss_03Btn_Cleared,
        Boss_04Btn_Cleared,
        Boss_05Btn_Cleared,
    }
    List<Image> _lockImgs = new List<Image>(BossCount);
    List<Image> _clearImgs = new List<Image>(BossCount);

    const int BossCount = 5;
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        for (int idx = 0; idx < BossCount; idx++)
        {
            _bossBtnTexts.Add(GetText((int)Texts.Boss_01BtnText + idx));
            _bossBtnTexts[idx].text = $"난이도 {idx + 1}: {Managers.Data.GetBossData(idx).BossName}";
        }

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.Boss_01Btn).gameObject).BindEvent(Btn_OnClickBoss_01);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.Boss_02Btn).gameObject).BindEvent(Btn_OnClickBoss_02);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.Boss_03Btn).gameObject).BindEvent(Btn_OnClickBoss_03);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.Boss_04Btn).gameObject).BindEvent(Btn_OnClickBoss_04);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.Boss_05Btn).gameObject).BindEvent(Btn_OnClickBoss_05);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.BossTryBtn).gameObject).BindEvent(Btn_OnClickBossTry);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        for (int i = 0; i < BossCount; i++)
        {
            _lockImgs.Add(GetImage((int)Images.Boss_01Btn_Lock + i));
            _clearImgs.Add(GetImage((int)Images.Boss_01Btn_Lock + i + BossCount));
        }

        InitialSetActiveFalse();
        return true;
    }

    // 초기에 비활성화할 오브젝트들
    void InitialSetActiveFalse()
    {
        int level = Managers.Game.BossLv;
        for (int i = 0; i < BossCount; i++)
        {
            if(i < level)
            {
                _lockImgs[i].gameObject.SetActive(false);
                _clearImgs[i].gameObject.SetActive(true);
            }
            else if(i > level)
            {
                _lockImgs[i].gameObject.SetActive(true);
                _clearImgs[i].gameObject.SetActive(false);
            }
            else
            {
                _lockImgs[i].gameObject.SetActive(false);
                _clearImgs[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region 버튼
    int _curBossId = -1;

    public void Btn_OnClickBoss_01() { _curBossId = (int)Buttons.Boss_01Btn; }
    public void Btn_OnClickBoss_02() { _curBossId = (int)Buttons.Boss_02Btn; }
    public void Btn_OnClickBoss_03() { _curBossId = (int)Buttons.Boss_03Btn; }
    public void Btn_OnClickBoss_04() { _curBossId = (int)Buttons.Boss_04Btn; }
    public void Btn_OnClickBoss_05() { _curBossId = (int)Buttons.Boss_05Btn; }

    public void Btn_OnClickBossTry() 
    { 
        // ToDo: 보스 씬 입장
        if(_curBossId < 0)
        {
            Managers.UI.OpenNotice(ConstValue.Notice_NotSelectBoss);
            return;
        }

        Managers.Game.EnterBossTry(_curBossId);
    }

    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}