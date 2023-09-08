using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdvManager
{
    RewardedAd _rewardedAd;

    // 테스트로 사용하는 ID들.
    const string TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    const string TEST_ANDROID_REWARDED = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_IOS_REWARDED = "ca-app-pub-3940256099942544/1712485313";

    // 실제 출시용 ID들
    const string ANDROID_APP_ID = "";
    const string IOS_APP_ID = "";

    #region 초기화
    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        LoadRewardedAd();
    }

    void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        // create our request used to load the ad.
        AdRequest adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(TEST_ANDROID_REWARDED, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

            _rewardedAd = ad;
        });

        PrepareAds();
    }

    void PrepareAds()
    {
        _rewardedAd.OnAdImpressionRecorded += HandleOnAdImpressionRecorded;
        _rewardedAd.OnAdClicked += HandleOnAdClicked;
        _rewardedAd.OnAdFullScreenContentOpened += HandleOnAdFullScreenContentOpened;
        _rewardedAd.OnAdFullScreenContentClosed += HandleOnAdFullScreenContentClosed;
        _rewardedAd.OnAdFullScreenContentFailed += HandleOnAdFullScreenContentFailed;
        _rewardedAd.OnAdPaid += HandleOnAdPaid;
    }
    #endregion

    #region 콜백
    public void HandleOnAdImpressionRecorded()
    {
        
    }

    public void HandleOnAdClicked()
    {
        
    }

    public void HandleOnAdFullScreenContentOpened()
    {
        
    }

    public void HandleOnAdFullScreenContentClosed()
    {
        LoadRewardedAd();
        Managers.Game.PlusAdCount(_curAdIdx);
    }

    public void HandleOnAdFullScreenContentFailed(AdError error)
    {
        
    }

    public void HandleOnAdPaid(AdValue value)
    {
        
    }
    #endregion

    #region 시청 및 보상
    int _curAdIdx = -1;

    // 해당 광고 시청 가능한지 체크
    public bool CanShowAd(int shopDataIdx, ConstValue.Ads targetAd)
    {
        int maxCount = Managers.Data.GetShopData(shopDataIdx).MaxCount;
        int curCount = Managers.Game.GetAdCount((int)targetAd);
        if(curCount < 0)
        {
            Debug.Log("Wrong Ad Count");
            return false;
        }

        _curAdIdx = (int)targetAd;
        return curCount < maxCount ? true : false;
    }

    // Reward 광고 시청
    public void ShowRewardedAds(Action<Reward> rewardedCallback)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
            _rewardedAd.Show(rewardedCallback);
        else
        {
            Managers.UI.OpenNotice(ConstValue.Notice_AdNotPrepared);
            LoadRewardedAd();
        }
            
    }
    #endregion
}