using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager
{
    RewardedAd _rewardedAd;

    // �׽�Ʈ�� ����ϴ� ID��.
    const string TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    const string TEST_ANDROID_REWARDED = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_IOS_REWARDED = "ca-app-pub-3940256099942544/1712485313";

    // ���� ��ÿ� ID��
    const string ANDROID_APP_ID = "";
    const string IOS_APP_ID = "";

    public bool AdPaid { get; private set; } = false;
    Action _rewardCallback;

    #region �ʱ�ȭ
    public void Init()
    {
        // ���� ���ɿ� ������Ʈ ����
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

        // ���� ��û�� AdRequest ����
        AdRequest adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // ���� �ε� ��û ����
        RewardedAd.Load(TEST_ANDROID_REWARDED, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // error�� null�� �ƴ϶�� �ε� ����
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

    #region �ݹ�
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

    #region ��û �� ����
    int _curAdIdx = -1;

    // �ش� ���� ��û �������� üũ
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

    // Reward ���� ��û
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

    // ����Ƽ Admob ���� �̽��� ���� Receiver�� Update���� üũ �� ���� ����
    // - ���� ��û�� ���� �����尡 �ƴ� �ٸ� �����忡�� ����
    // - �׷��� ���� ������ ���� �ݱ� ���� �����
    // - ���, Admob�� �ݹ� ����� �̿��� ���� ���� �� UnityException �߻�(### can only be called from the main thread.)
    public void GetReward()
    {
        // ���� ����
        _rewardCallback?.Invoke();
        // �ʱ�ȭ
        AdPaid = false;
        _rewardCallback = null;
    }
    #endregion
}