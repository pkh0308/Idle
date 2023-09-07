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
        CalOfflineReward();

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }

    const int Days = 365;
    const int Hours = 24;
    const int Minutes = 60;
    void CalOfflineReward()
    {
        int year = DateTime.Now.Year - Managers.Game.LastAccessYear;
        int day = (DateTime.Now.DayOfYear + (year * Days)) - Managers.Game.LastAccessDayOfYear;
        int minute = (day * Hours * Minutes) + (DateTime.Now.Hour * Minutes + DateTime.Now.Minute) - (Managers.Game.LastAccessMinutes);
        _offlineTimeText.text = string.Format("오프라인 시간: {0:n00}시간 {1:n00}분", minute / 60, minute % 60); 

        int testVal = minute * Managers.Game.GetRewardPerMinute();
        _rewardText.text = Custom.CalUnit(testVal);
        Managers.Game.GetGold(testVal);
    }

    #region 버튼
    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion
}