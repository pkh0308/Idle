using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager
{
    RewardedAd _rewardedAd;

    // 테스트로 사용하는 ID들.
    const string TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    const string TEST_ANDROID_REWARDED = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_IOS_REWARDED = "ca-app-pub-3940256099942544/1712485313";

    // 실제 출시용 ID들
    const string ANDROID_APP_ID = "";
    const string IOS_APP_ID = "";

    public bool AdPaid { get; private set; } = false;
    Action _rewardCallback;

    #region 초기화
    public void Init()
    {
        // 보상 수령용 오브젝트 생성
        GameObject rewardReceiver = new GameObject() { name = "AdRewardReceiver" };
        UnityEngine.Object.DontDestroyOnLoad(rewardReceiver);
        rewardReceiver.AddComponent<RewardReceiver>();

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

        // 광고 요청용 AdRequest 생성
        AdRequest adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // 광고 로드 요청 전송
        RewardedAd.Load(TEST_ANDROID_REWARDED, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // error가 null이 아니라면 로드 실패
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            _rewardedAd = ad;
            PrepareAds();
        });
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
        Debug.Log("### AdImpressionRecorded");
    }

    public void HandleOnAdClicked()
    {
        Debug.Log("### AdClicked");
    }

    public void HandleOnAdFullScreenContentOpened()
    {
        Debug.Log("### AdFullScreenContentOpened");
    }

    public void HandleOnAdFullScreenContentClosed()
    {
        Debug.Log("### AdFullScreenContentClosed");
        LoadRewardedAd();
        Managers.Game.PlusAdCount(_curAdIdx);
    }

    public void HandleOnAdFullScreenContentFailed(AdError error)
    {
        Debug.Log("### AdFullScreenContentFailed");
    }

    public void HandleOnAdPaid(AdValue value)
    {
        Debug.Log("### AdPaid");
        AdPaid = true;
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
            Debug.Log("### Wrong Ad Count");
            return false;
        }

        _curAdIdx = (int)targetAd;
        return curCount < maxCount ? true : false;
    }

    // Reward 광고 시청
    public void ShowRewardedAds(Action rewardCallback, Action<Reward> rewardEarnedCallback = null)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardCallback = rewardCallback;
            _rewardedAd.Show(rewardEarnedCallback);
        }
        else
        {
            Managers.UI.OpenNotice(ConstValue.Notice_AdNotPrepared);
            LoadRewardedAd();
        }
            
    }

    // 유니티 Admob 관련 이슈로 인해 Receiver의 Update에서 체크 후 보상 지급
    // - 광고 시청은 메인 스레드가 아닌 다른 스레드에서 실행
    // - 그런데 보상 지급은 광고를 닫기 전에 실행됨
    // - 결과, Admob의 콜백 기능을 이용해 보상 수령 시 UnityException 발생(### can only be called from the main thread.)
    public void GetReward()
    {
        // 보상 지급
        _rewardCallback?.Invoke();
        // 초기화
        AdPaid = false;
        _rewardCallback = null;
    }
    #endregion
}