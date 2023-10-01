using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class GameManager
{
    public enum GameState
    {
        Title,
        Main, 
        Boss,
        Clear
    }
    public GameState CurState { get; private set; }

    enum Scenes
    {
        TitleScene,
        MainScene,
        BossScene,
        EndingScene
    }

    public void Init()
    {
        CurState = GameState.Title;
    }

    #region GameData
    public int AtkPowerLv { get; private set; }
    public int AtkSpeedLv { get; private set; }
    public int CritChanceLv { get; private set; }
    public int CritDamageLv { get; private set; }
    public int GoldUpLv { get; private set; }

    public int WeaponLv { get; private set; }

    public int Tr_AtkPowerLv { get; private set; }
    public int Tr_AtkSpeedLv { get; private set; }
    public int Tr_CritChanceLv { get; private set; }
    public int Tr_CritDamageLv { get; private set; }
    public int Tr_GoldUpLv { get; private set; }

    public string NickName { get; private set; }
    public int CurGold { get; private set; }
    public int CurGem { get; private set; }
    public int CurStageIdx { get; private set; }

    public int LastAccessYear { get; private set; }
    public int LastAccessDayOfYear { get; private set; }
    public int LastAccessMinutes { get; private set; }

    public int AdCount_Gold_2hr { get; private set; }
    public int AdCount_Gem_100 { get; private set; }

    public int BossLv { get; private set; }

    void GetGameDataFromDataManager()
    {
        GameData data = Managers.Data.CurGameData;
        // 강화 레벨
        AtkPowerLv = data.AtkPowLv;
        AtkSpeedLv = data.AtkSpdLv;
        CritChanceLv = data.CritChanceLv;
        CritDamageLv = data.CritDmgLv;
        GoldUpLv = data.GoldUpLv;
        // 무기 레벨
        WeaponLv = data.WeaponLv;
        // 보물 레벨
        Tr_AtkPowerLv = data.Treasure_AtkPowLv;
        Tr_AtkSpeedLv = data.Treasure_AtkSpdLv;
        Tr_CritChanceLv = data.Treasure_CritChanceLv;
        Tr_CritDamageLv = data.Treasure_CritDmgLv;
        Tr_GoldUpLv = data.Treasure_GoldUpLv;
        // 닉네임, 골드, 보석, 스테이지 인덱스
        NickName = data.NickName;
        CurGold = data.CurGold;
        CurGem = data.CurGem;
        CurStageIdx = data.StageIdx;
        // 마지막 접속 연도, 날짜, 시간(분)
        LastAccessYear = data.LastAccessYear;
        LastAccessDayOfYear = data.LastAccessDayOfYear;
        LastAccessMinutes = data.LastAccessMinutes;
        // 광고 카운트
        // 마지막 접속과 같은 날이라면 카운트 유지, 아니라면 초기화
        AdCount_Gold_2hr = DateTime.Now.DayOfYear == LastAccessDayOfYear ? data.AdCount_Gold_2hr : 0;
        AdCount_Gem_100 = DateTime.Now.DayOfYear == LastAccessDayOfYear ? data.AdCount_Gem_100 : 0;
        // 보스 레벨
        BossLv = data.BossLv;
    }

    public void UpdateGameData()
    {
        if (CurState == GameState.Title || CurState == GameState.Clear)
            return;

        // 마지막 접속과 같은 날이라면 카운트 유지, 아니라면 초기화
        AdCount_Gold_2hr = DateTime.Now.DayOfYear == LastAccessDayOfYear ? AdCount_Gold_2hr : 0;
        AdCount_Gem_100 = DateTime.Now.DayOfYear == LastAccessDayOfYear ? AdCount_Gem_100 : 0;
        // 날짜 및 시간 갱신
        LastAccessYear = DateTime.Now.Year;
        LastAccessDayOfYear = DateTime.Now.DayOfYear;
        LastAccessMinutes = (DateTime.Now.Hour * ConstValue.Minutes) + DateTime.Now.Minute;

        GameData data = new GameData()
        {
            AtkPowLv = AtkPowerLv,
            AtkSpdLv = AtkSpeedLv,
            CritChanceLv = CritChanceLv,
            CritDmgLv = CritDamageLv,
            GoldUpLv = GoldUpLv,

            WeaponLv = WeaponLv,

            Treasure_AtkPowLv = Tr_AtkPowerLv,
            Treasure_AtkSpdLv = Tr_AtkSpeedLv,
            Treasure_CritChanceLv = Tr_CritChanceLv,
            Treasure_CritDmgLv = Tr_CritDamageLv,
            Treasure_GoldUpLv = Tr_GoldUpLv,

            NickName = this.NickName,

            CurGold = this.CurGold,
            CurGem = this.CurGem,
            StageIdx = this.CurStageIdx,
            LastAccessYear = this.LastAccessYear,
            LastAccessDayOfYear = this.LastAccessDayOfYear,
            LastAccessMinutes = this.LastAccessMinutes,
            AdCount_Gold_2hr = this.AdCount_Gold_2hr,
            AdCount_Gem_100 = this.AdCount_Gem_100,

            BossLv = this.BossLv,
        };
        Managers.Data.SetGameData(data);
    }
    #endregion

    #region 게임 시작
    public void StartNewGame(string nickname)
    {
        Managers.Data.CreateNewGameData(nickname);
        GetGameDataFromDataManager();
        CurState = GameState.Main;
        UpdatePlayerStatus();
        LoadScene(Scenes.MainScene.ToString());
    }

    public bool LoadGame()
    {
        if (Managers.Data.CurGameData == null || Managers.Data.CurGameData.NickName == null)
            return false;

        // 클리어 데이터라면 없는 것으로 취급
        GetGameDataFromDataManager();
        if (CurStageIdx > Managers.Data.MaxStageIdx)
            return false;

        CurState = GameState.Main;
        UpdatePlayerStatus();
        LoadScene(Scenes.MainScene.ToString());
        return true;
    }

    public void BackToTitle()
    {
        UpdateGameData();
        Managers.Data.SaveGameData();
        LoadScene(Scenes.TitleScene.ToString());
        CurState = GameState.Title;
    }

    public void BackToMain()
    {
        UpdatePlayerStatus();
        LoadScene(Scenes.MainScene.ToString());
    }
    #endregion

    #region 씬 이동
    void LoadScene(string sceneName)
    {
        Managers.UI.ClearStack();
        Managers.Resc.Clear();
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region 재화 획득 & 소모
    Action _goleGemUpdate = null;
    public void SetCallBackForGoods(Action callback) { _goleGemUpdate = callback; }
    void UpdateGoldGem() { _goleGemUpdate?.Invoke(); }

    bool Purchase(ConstValue.Goods type, int value)
    {
        int temp = 0;
        if(type == ConstValue.Goods.Gold)
            temp = CurGold;
        else if(type == ConstValue.Goods.Gem)
            temp = CurGem;

        temp -= value;
        if (temp < 0)
            return false;

        if (type == ConstValue.Goods.Gold)
            CurGold = temp;
        else if (type == ConstValue.Goods.Gem)
            CurGem = temp;

        UpdateGoldGem();
        return true;
    }

    public int GetGold(int value) 
    { 
        CurGold += value;
        UpdateGoldGem();
        return CurGold; 
    }
    public int GetGem(int value) 
    { 
        CurGem += value; 
        UpdateGoldGem(); 
        return CurGem; 
    }

    // 오프라인 보상(1분당)
    public int GetRewardPerMinute() { return _stageData.DropGold; }
    public void GetGoldPerHour(int hour) { GetGold(_stageData.DropGold * 60 * hour); }

    public int GetOfflineReward(out int minutes)
    {
        int year = DateTime.Now.Year - LastAccessYear;
        int day = (DateTime.Now.DayOfYear + (year * ConstValue.Days)) - LastAccessDayOfYear;
        int min = (day * ConstValue.Hours * ConstValue.Minutes) + (DateTime.Now.Hour * ConstValue.Minutes + DateTime.Now.Minute) - LastAccessMinutes;
        minutes = min;
        LastAccessMinutes = 0; // 오프라인 보상 중복 수령 방지

        int offlineRwd = min * GetRewardPerMinute();
        GetGold(offlineRwd);
        return offlineRwd;
    }
    #endregion

    #region 전투
    StageData _stageData;
    int _atkPow;
    int _atkSpd;
    int _critChance;
    int _critDmg;
    int _goldUp;

    public int EnemyHp { get; private set; }
    public int MaxEnemyHp { get; private set; }

    public void UpdatePlayerStatus()
    {
        WeaponData wData = Managers.Data.GetWeaponData(WeaponLv);
        float tr_atkPow = 1 + Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_AtkPow, Tr_AtkPowerLv) / 10000.0f;
        float tr_atkSpd = 1 + Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_AtkSpd, Tr_AtkSpeedLv) / 10000.0f;
        float tr_critChance = 1 + Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_CritChance, Tr_CritChanceLv) / 10000.0f;
        float tr_critDmg = 1 + Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_CritDmg, Tr_CritDamageLv) / 10000.0f;
        float tr_goldUp = 1 + Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_GoldUp, Tr_GoldUpLv) / 10000.0f;

        _atkPow = (int)((Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkPow, AtkPowerLv) + wData.AtkPower) * tr_atkPow); 
        _atkSpd = (int)(Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv) * tr_atkSpd);
        _critChance = (int)((Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritChance, CritChanceLv) + wData.CritChance) * tr_critChance); 
        _critDmg = (int)((Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritDmg, CritDamageLv) + wData.CritDamage) * tr_critDmg); 
        _goldUp = (int)(Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.GoldUp, GoldUpLv) * tr_goldUp); 
    }

    public int CurEnemyCount { get; private set; }
    public int MaxEnemyCount { get; private set; }
    int[] _enemyIds;
    public void InitStage()
    {
        // 게임 오버
        if(CurStageIdx > Managers.Data.MaxStageIdx)
        {
            GameOver();
            return;
        }

        _stageData = Managers.Data.GetStageData(CurStageIdx);
        MaxEnemyHp = _stageData.Hp;
        EnemyHp = MaxEnemyHp;
        MaxEnemyCount = _stageData.EnemyCount;
        CurEnemyCount = 0;

        _enemyIds = new int[_stageData.EnemyCount];
        for(int i = 0; i < _enemyIds.Length; i++)
            _enemyIds[i] = Random.Range(_stageData.MinEnemyId, _stageData.MaxEnemyId + 1);
    }

    public int Attack(out bool critical, bool cheerBuffOn = false)
    {
        critical = Random.Range(0, 10000) < _critChance;
        int dmg = cheerBuffOn ? Convert.ToInt32(_atkPow * ConstValue.CheerBuffRate) : _atkPow;
        if(critical) 
            dmg = Convert.ToInt32(dmg * (_critDmg / 10000.0f));

        EnemyHp = EnemyHp > dmg ? EnemyHp - dmg : 0;
        return dmg;
    }

    public bool OnEnemyDie()
    {
        GetGoldFromEnemy();
        GetGemFromEnemy();
        EnemyHp = MaxEnemyHp;

        CurEnemyCount++;
        if(CurEnemyCount == MaxEnemyCount)
        {
            CurStageIdx++;
            return true;
        }
        return false;
    }

    public int GetAttackDelay() { return _atkSpd; }

    public void GetGoldFromEnemy() { CurGold += (int)(_stageData.DropGold * (_goldUp / 10000.0f)); }
    public void GetGemFromEnemy() { CurGem += _stageData.DropGem; }

    public int GetEnemyId(int idx) { return idx < MaxEnemyCount ? _enemyIds[idx] : -1; }
    public int GetCurEnemyId() { return _enemyIds[CurEnemyCount]; }
    #endregion

    #region 강화
    bool Enhance(int idx, int level)
    {
        int cost = Managers.Data.GetEnahnceCost(idx, level);
        if(cost < 0)
        {
            Debug.Log($"Wrong Enhance idx or level: {idx}, {level}");
            return false;
        }

        if (Purchase(ConstValue.Goods.Gold, cost) == false)
            return false;

        // 해당하는 강화 레벨 증가
        switch (idx)
        {
            case (int)ConstValue.Enhances.AtkPow:
                AtkPowerLv++;
                break;
            case (int)ConstValue.Enhances.AtkSpd:
                AtkSpeedLv++;
                break;
            case (int)ConstValue.Enhances.CritChance:
                CritChanceLv++;
                break;
            case (int)ConstValue.Enhances.CritDmg:
                CritDamageLv++;
                break;
            case (int)ConstValue.Enhances.GoldUp:
                GoldUpLv++;
                break;
        }
        UpdatePlayerStatus();
        return true;
    }

    public bool EnhanceAtkPow() { return Enhance((int)ConstValue.Enhances.AtkPow, AtkPowerLv); }
    public bool EnhanceAtkSpd() { return Enhance((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv); }
    public bool EnhanceCritChance() { return Enhance((int)ConstValue.Enhances.CritChance, CritChanceLv); }
    public bool EnhanceCritDamage() { return Enhance((int)ConstValue.Enhances.CritDmg, CritDamageLv); }
    public bool EnhanceGoldUp() { return Enhance((int)ConstValue.Enhances.GoldUp, GoldUpLv); }
    #endregion

    #region 무기
    public bool BuyWeapon(int level)
    {
        if (level < WeaponLv)
            return false;

        WeaponData wData = Managers.Data.GetWeaponData(level);
        if (wData == null)
        {
            Debug.Log($"Wrong weaponLv to buy: {WeaponLv}");
            return false;
        }

        if (Purchase(ConstValue.Goods.Gold, wData.Cost) == false)
            return false;

        WeaponLv = level + 1;
        UpdatePlayerStatus();
        return true;
    }
    #endregion

    #region 보물
    bool BuyTreasure(int idx, int level)
    {
        int cost = Managers.Data.GetTreasureCost(idx, level);
        if (cost < 0)
        {
            Debug.Log($"Wrong Enhance idx or level: {idx}, {level}");
            return false;
        }

        if (Purchase(ConstValue.Goods.Gem, cost) == false)
            return false;

        // 해당하는 강화 레벨 증가
        switch(idx)
        {
            case (int)ConstValue.Treasures.Tr_AtkPow:
                Tr_AtkPowerLv++;
                break;
            case (int)ConstValue.Treasures.Tr_AtkSpd:
                Tr_AtkSpeedLv++;
                break;
            case (int)ConstValue.Treasures.Tr_CritChance:
                Tr_CritChanceLv++;
                break;
            case (int)ConstValue.Treasures.Tr_CritDmg:
                Tr_CritDamageLv++;
                break;
            case (int)ConstValue.Treasures.Tr_GoldUp:
                Tr_GoldUpLv++;
                break;
        }
        UpdatePlayerStatus();
        return true;
    }

    public bool BuyTreasure_AtkPow() { return BuyTreasure((int)ConstValue.Treasures.Tr_AtkPow, Tr_AtkPowerLv); }
    public bool BuyTreasure_AtkSpd() { return BuyTreasure((int)ConstValue.Treasures.Tr_AtkSpd, Tr_AtkSpeedLv); }
    public bool BuyTreasure_CritChance() { return BuyTreasure((int)ConstValue.Treasures.Tr_CritChance, Tr_CritChanceLv); }
    public bool BuyTreasure_CritDmg() { return BuyTreasure((int)ConstValue.Treasures.Tr_CritDmg, Tr_CritDamageLv); }
    public bool BuyTreasure_GoldUp() { return BuyTreasure((int)ConstValue.Treasures.Tr_GoldUp, Tr_GoldUpLv); }
    #endregion

    #region 상점
    public bool BuyShopGoods(int idx)
    {
        ShopData data = Managers.Data.GetShopData(idx);
        if(data.MaxCount > 0) // 광고 시청 상품
        {
            
        }
        else
        {

        }

        return true;
    }
    #endregion

    #region 광고
    public int GetAdCount(int idx)
    {
        if(LastAccessDayOfYear != DateTime.Now.DayOfYear)
        {
            LastAccessDayOfYear = DateTime.Now.DayOfYear;
            ResetAdCount();
        }

        switch(idx)
        {
            case (int)ConstValue.Ads.Gold_2hr:
                return AdCount_Gold_2hr;
            case (int)ConstValue.Ads.Gem_100:
                return AdCount_Gem_100;
            default:
                return -1;
        }
    }

    public void PlusAdCount(int idx)
    {
        switch (idx)
        {
            case (int)ConstValue.Ads.Gold_2hr:
                AdCount_Gold_2hr++;
                break;
            case (int)ConstValue.Ads.Gem_100:
                AdCount_Gem_100++;
                break;
        }
    }

    // 날짜 넘어간 경우 호출
    void ResetAdCount()
    {
        AdCount_Gold_2hr = 0;
        AdCount_Gem_100 = 0;
    }
    #endregion

    #region 보스
    public BossData CurBossData { get; private set; }
    public void EnterBossTry(int idx)
    {
        CurBossData = Managers.Data.GetBossData(idx);
        if(CurBossData == null)
        {
            Debug.Log($"Wrong boss idx: {idx}");
            return;
        }

        CurState = GameState.Boss;
        LoadScene(Scenes.BossScene.ToString());
    }

    public void InitBoss()
    {
        MaxEnemyHp = CurBossData.Hp;
        EnemyHp = CurBossData.Hp;
    }

    public void DefeatBoss()
    {
        BossLv++;
        GetGoldAndGemFromBoss();
    }

    void GetGoldAndGemFromBoss() 
    {
        CurGem += CurBossData.DropGem;
        CurGold += CurBossData.DropGold;
    }

    public void ExitBossTry()
    {
        BackToMain();
    }
    #endregion

    #region 게임 오버
    void GameOver()
    {
        UpdateGameData();
        Managers.Data.SaveGameData();
        LoadScene(Scenes.EndingScene.ToString());
        CurState = GameState.Clear;
    }
    #endregion
}