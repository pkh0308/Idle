using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class GameManager
{
    enum GameState
    {
        Title,
        Main, 
        Boss
    }
    GameState curState;

    enum Scenes
    {
        TitleScene,
        MainScene,
        BossScene
    }

    public void Init()
    {
        curState = GameState.Title;
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
        // ��ȭ ����
        AtkPowerLv = data.AtkPowLv;
        AtkSpeedLv = data.AtkSpdLv;
        CritChanceLv = data.CritChanceLv;
        CritDamageLv = data.CritDmgLv;
        GoldUpLv = data.GoldUpLv;
        // ���� ����
        WeaponLv = data.WeaponLv;
        // ���� ����
        Tr_AtkPowerLv = data.Treasure_AtkPowLv;
        Tr_AtkSpeedLv = data.Treasure_AtkSpdLv;
        Tr_CritChanceLv = data.Treasure_CritChanceLv;
        Tr_CritDamageLv = data.Treasure_CritDmgLv;
        Tr_GoldUpLv = data.Treasure_GoldUpLv;
        // �г���, ���, ����, �������� �ε���
        NickName = data.NickName;
        CurGold = data.CurGold;
        CurGem = data.CurGem;
        CurStageIdx = data.StageIdx;
        // ������ ���� ����, ��¥, �ð�(��)
        LastAccessYear = data.LastAccessYear;
        LastAccessDayOfYear = data.LastAccessDayOfYear;
        LastAccessMinutes = data.LastAccessMinutes;
        // ���� ī��Ʈ
        // ������ ���Ӱ� ���� ���̶�� ī��Ʈ ����, �ƴ϶�� �ʱ�ȭ
        AdCount_Gold_2hr = DateTime.Now.DayOfYear == LastAccessDayOfYear ? data.AdCount_Gold_2hr : 0;
        AdCount_Gem_100 = DateTime.Now.DayOfYear == LastAccessDayOfYear ? data.AdCount_Gem_100 : 0;
        // ���� ����
        BossLv = data.BossLv;
    }

    public void UpdateGameData()
    {
        if (curState == GameState.Title)
            return;

        // ������ ���Ӱ� ���� ���̶�� ī��Ʈ ����, �ƴ϶�� �ʱ�ȭ
        AdCount_Gold_2hr = DateTime.Now.DayOfYear == LastAccessDayOfYear ? AdCount_Gold_2hr : 0;
        AdCount_Gem_100 = DateTime.Now.DayOfYear == LastAccessDayOfYear ? AdCount_Gem_100 : 0;
        // ��¥ �� �ð� ����
        LastAccessYear = DateTime.Now.Year;
        LastAccessDayOfYear = DateTime.Now.DayOfYear;
        LastAccessMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

        GameData data = new GameData(AtkPowerLv, AtkSpeedLv, CritChanceLv, CritDamageLv, GoldUpLv, WeaponLv,
                                     Tr_AtkPowerLv, Tr_AtkSpeedLv, Tr_CritChanceLv, Tr_CritDamageLv, Tr_GoldUpLv,
                                     NickName, CurGold, CurGem, CurStageIdx, LastAccessYear, LastAccessDayOfYear, LastAccessMinutes, 
                                     AdCount_Gold_2hr, AdCount_Gem_100, BossLv);
        Managers.Data.SetGameData(data);
    }
    #endregion

    #region ���� ����
    public void StartNewGame(string nickname)
    {
        Managers.Data.CreateNewGameData(nickname);
        GetGameDataFromDataManager();
        curState = GameState.Main;
        InitPlayer();
        SceneManager.LoadScene(Scenes.MainScene.ToString());
    }

    public bool LoadGame()
    {
        if (Managers.Data.CurGameData == null)
            return false;

        GetGameDataFromDataManager();
        curState = GameState.Main;
        InitPlayer();
        SceneManager.LoadScene(Scenes.MainScene.ToString());
        return true;
    }

    public void BackToTitle()
    {
        UpdateGameData();
        Managers.Data.SaveGameData();
        SceneManager.LoadScene(Scenes.TitleScene.ToString());
    }

    public void BackToMain()
    {
        curState = GameState.Main;
        InitPlayer();
        SceneManager.LoadScene(Scenes.MainScene.ToString());
    }
    #endregion

    #region ��ȭ ȹ�� & �Ҹ�
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

    // �������� ����(1�д�)
    public int GetRewardPerMinute() { return _stageData.DropGold; }
    public void GetGoldPerHour(int hour) { GetGold(_stageData.DropGold * 60 * hour); }
    #endregion

    #region ����
    StageData _stageData;
    int _atkPow;
    int _atkSpd;
    int _critChance;
    int _critDmg;
    int _goldUp;

    public int EnemyHp { get; private set; }
    public int MaxEnemyHp { get; private set; }

    public void InitPlayer()
    {
        _atkPow = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkPow, AtkPowerLv);
        _atkSpd = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv);
        _critChance = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritChance, CritChanceLv);
        _critDmg = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritDmg, CritDamageLv);
        _goldUp = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.GoldUp, GoldUpLv);
    }

    public int CurEnemyCount { get; private set; }
    public int MaxEnemyCount { get; private set; }
    int[] _enemyIds;
    public void InitStage()
    {
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
        WeaponData wData = Managers.Data.GetWeaponData(WeaponLv);
        critical = Random.Range(0, 10000) < _critChance + wData.CritChance;
        int dmg = cheerBuffOn ? Convert.ToInt32((_atkPow + wData.AtkPower) * ConstValue.CheerUpRate) : _atkPow + wData.AtkPower;
        if(critical) 
            dmg = Convert.ToInt32(dmg * ((_critDmg + wData.CritDamage) / 10000.0f));

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

    public int GetAttackDelay()
    {
        int tr_atkSpeed = Managers.Data.GetTreasureValue((int)ConstValue.Treasures.Tr_AtkPow, Tr_AtkSpeedLv);
        return Convert.ToInt32(_atkSpd * (1 + (tr_atkSpeed / 10000.0f)));
    }

    public void GetGoldFromEnemy() { CurGold += (int)(_stageData.DropGold * (_goldUp / 10000.0f)); }
    public void GetGemFromEnemy() { CurGem += _stageData.DropGem; }

    public int GetEnemyId(int idx) { return idx < MaxEnemyCount ? _enemyIds[idx] : -1; }
    public int GetCurEnemyId() { return _enemyIds[CurEnemyCount]; }
    #endregion

    #region ��ȭ
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

        // �ش��ϴ� ��ȭ ���� ����
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
        return true;
    }

    public bool EnhanceAtkPow() { return Enhance((int)ConstValue.Enhances.AtkPow, AtkPowerLv); }
    public bool EnhanceAtkSpd() { return Enhance((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv); }
    public bool EnhanceCritChance() { return Enhance((int)ConstValue.Enhances.CritChance, CritChanceLv); }
    public bool EnhanceCritDamage() { return Enhance((int)ConstValue.Enhances.CritDmg, CritDamageLv); }
    public bool EnhanceGoldUp() { return Enhance((int)ConstValue.Enhances.GoldUp, GoldUpLv); }
    #endregion

    #region ����
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
        return true;
    }
    #endregion

    #region ����
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

        // �ش��ϴ� ��ȭ ���� ����
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
        return true;
    }

    public bool BuyTreasure_AtkPow() { return BuyTreasure((int)ConstValue.Treasures.Tr_AtkPow, Tr_AtkPowerLv); }
    public bool BuyTreasure_AtkSpd() { return BuyTreasure((int)ConstValue.Treasures.Tr_AtkSpd, Tr_AtkSpeedLv); }
    public bool BuyTreasure_CritChance() { return BuyTreasure((int)ConstValue.Treasures.Tr_CritChance, Tr_CritChanceLv); }
    public bool BuyTreasure_CritDmg() { return BuyTreasure((int)ConstValue.Treasures.Tr_CritDmg, Tr_CritDamageLv); }
    public bool BuyTreasure_GoldUp() { return BuyTreasure((int)ConstValue.Treasures.Tr_GoldUp, Tr_GoldUpLv); }
    #endregion

    #region ����
    public bool BuyShopGoods(int idx)
    {
        ShopData data = Managers.Data.GetShopData(idx);
        if(data.MaxCount > 0) // ���� ��û ��ǰ
        {
            
        }
        else
        {

        }

        return true;
    }
    #endregion

    #region ����
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

    // ��¥ �Ѿ ��� ȣ��
    void ResetAdCount()
    {
        AdCount_Gold_2hr = 0;
        AdCount_Gem_100 = 0;
    }
    #endregion

    #region ����
    public BossData CurBossData { get; private set; }
    public void EnterBossTry(int idx)
    {
        CurBossData = Managers.Data.GetBossData(idx);
        if(CurBossData == null)
        {
            Debug.Log($"Wrong boss idx: {idx}");
            return;
        }

        curState = GameState.Boss;
        SceneManager.LoadScene(Scenes.BossScene.ToString());
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
}