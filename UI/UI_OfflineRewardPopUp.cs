using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UI_OfflineRewardPopUp : UI_PopUp
{
    enum Texts
    {
        OfflineTitleText,
        OfflineTimeText,
        RewardText,
        ExitBtnText
    }
    TextMeshProUGUI _offlineTimeText;
    TextMeshProUGUI _rewardText;

    enum Buttons
    {
        ExitBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _offlineTimeText = GetText((int)Texts.OfflineTimeText);
        _rewardText = GetText((int)Texts.RewardText);
        TextUpdate();

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }

    void TextUpdate()
    {
        int reward = Managers.Game.GetOfflineReward(out int minutes);
        _rewardText.text = Custom.CalUnit(reward);
        _offlineTimeText.text = string.Format("오프라인 시간: {0:00}시간 {1:00}분", minutes / 60, minutes % 60);
        
    }

    #region 버튼
    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}