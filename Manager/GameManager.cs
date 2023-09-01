using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    enum GameState
    {
        Title,
        Main
    }
    GameState curState;

    enum Scenes
    {
        TitleScene,
        MainScene
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

    void GetGameDataFromDataManager()
    {
        GameData data = Managers.Data.CurGameData;

        AtkPowerLv = data.AtkPowLv;
        AtkSpeedLv = data.AtkSpdLv;
        CritChanceLv = data.CritChanceLv;
        CritDamageLv = data.CritDmgLv;
        GoldUpLv = data.GoldUpLv;

        WeaponLv = data.WeaponLv;

        Tr_AtkPowerLv = data.Treasure_AtkPowLv;
        Tr_AtkSpeedLv = data.Treasure_AtkSpdLv;
        Tr_CritChanceLv = data.Treasure_CritChanceLv;
        Tr_CritDamageLv = data.Treasure_CritDmgLv;
        Tr_GoldUpLv = data.Treasure_GoldUpLv;

        NickName = data.NickName;
        CurGold = data.CurGold;
        CurGem = data.CurGem;
        CurStageIdx = data.StageIdx;
    }

    public void UpdateGameData()
    {
        if (curState == GameState.Title)
            return;

        GameData data = new GameData(AtkPowerLv, AtkSpeedLv, CritChanceLv, CritDamageLv, GoldUpLv, WeaponLv,
                                     Tr_AtkPowerLv, Tr_AtkSpeedLv, Tr_CritChanceLv, Tr_CritDamageLv, Tr_GoldUpLv,
                                     NickName, CurGold, CurGem, CurStageIdx);
        Managers.Data.SetGameData(data);
    }
    #endregion

    #region 게임 시작
    public void StartNewGame(string nickname)
    {
        Managers.Data.CreateNewGameData(nickname);
        GetGameDataFromDataManager();
        curState = GameState.Main;
        InitStage();
        UpdatePlayer();
        SceneManager.LoadScene(Scenes.MainScene.ToString());
    }

    public bool LoadGame()
    {
        if (Managers.Data.CurGameData == null)
            return false;

        GetGameDataFromDataManager();
        curState = GameState.Main;
        InitStage();
        UpdatePlayer();
        SceneManager.LoadScene(Scenes.MainScene.ToString());
        return true;
    }
    #endregion

    #region 재화 획득 & 소모
    public bool Purchase(ConstValue.Goods type, int value)
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

        return true;
    }

    public int GetGold(int value) { CurGold += value; return CurGold; }
    public int GetGem(int value) { CurGem += value; return CurGem; }

    
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
    int _curEnemyCount;
    int _maxEnemyCount;

    public void UpdatePlayer()
    {
        _atkPow = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkPow, AtkPowerLv);
        _atkSpd = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv);
        _critChance = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritChance, CritChanceLv);
        _critDmg = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.CritDmg, CritDamageLv);
        _goldUp = Managers.Data.GetEnahnceValue((int)ConstValue.Enhances.GoldUp, GoldUpLv);
    }

    int[] _enemyIds;
    public void InitStage()
    {
        _stageData = Managers.Data.GetStageData(CurStageIdx);
        MaxEnemyHp = _stageData.Hp;
        EnemyHp = MaxEnemyHp;
        _maxEnemyCount = _stageData.EnemyCount;
        _curEnemyCount = 0;

        _enemyIds = new int[_stageData.EnemyCount];
        for(int i = 0; i < _enemyIds.Length; i++)
            _enemyIds[i] = Random.Range(_stageData.MinEnemyId, _stageData.MaxEnemyId + 1);
    }

    public void Attack()
    {
        WeaponData wData = Managers.Data.GetWeaponData(WeaponLv);
        bool critical = Random.Range(0, 10000) < _critChance + wData.CritChance;
        int dmg = _atkPow + wData.AtkPower;
        if(critical) 
            dmg = (int)(dmg * (_critDmg + wData.CritDamage / 10000.0f));

        EnemyHp -= dmg;
    }

    public bool OnEnemyDie()
    {
        GetGoldFromEnemy();
        GetGemFromEnemy();
        EnemyHp = MaxEnemyHp;

        _curEnemyCount++;
        if(_curEnemyCount == _maxEnemyCount)
        {
            CurStageIdx++;
            InitStage();
            return true;
        }
        return false;
    }

    public void GetGoldFromEnemy() { CurGold += (int)(_stageData.DropGold * (_goldUp / 10000.0f)); }
    public void GetGemFromEnemy() { CurGem += _stageData.DropGem; }

    public int GetCurEnemyId() { return _enemyIds[_curEnemyCount]; }
    public int GetNextEnemyId() { return _curEnemyCount + 1 < _maxEnemyCount ? _enemyIds[_curEnemyCount + 1] : -1; }
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
}