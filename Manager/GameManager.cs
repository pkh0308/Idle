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

    #region GameData 관리
    public int AtkPowerLv { get; private set; }
    public int AtkSpeedLv { get; private set; }
    public int CritChanceLv { get; private set; }
    public int CritDamageLv { get; private set; }
    public int GoldUpLv { get; private set; }

    public int WeaponLv { get; private set; }

    public int Tr_atkPowerLv { get; private set; }
    public int Tr_atkSpeedLv { get; private set; }
    public int Tr_critChanceLv { get; private set; }
    public int Tr_critDamageLv { get; private set; }
    public int Tr_goldUpLv { get; private set; }

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

        Tr_atkPowerLv = data.Treasure_AtkPowLv;
        Tr_atkSpeedLv = data.Treasure_AtkSpdLv;
        Tr_critChanceLv = data.Treasure_CritChanceLv;
        Tr_critDamageLv = data.Treasure_CritDmgLv;
        Tr_goldUpLv = data.Treasure_GoldUpLv;

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
                                     Tr_atkPowerLv, Tr_atkSpeedLv, Tr_critChanceLv, Tr_critDamageLv, Tr_goldUpLv,
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
    
    public void InitStage()
    {
        _stageData = Managers.Data.GetStageData(CurStageIdx);
        MaxEnemyHp = _stageData.Hp;
        EnemyHp = MaxEnemyHp;
        _maxEnemyCount = _stageData.EnemyCount;
        _curEnemyCount = 0;
    }

    public void Attack()
    {
        bool critical = Random.Range(0, 10000) < _critChance;
        int dmg = _atkPow;
        if(critical) 
            dmg = (int)(dmg * (_critDmg / 10000.0f));

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

        return true;
    }

    public bool EnhanceAtkPow() { return Enhance((int)ConstValue.Enhances.AtkPow, AtkPowerLv); }
    public bool EnhanceAtkSpd() { return Enhance((int)ConstValue.Enhances.AtkSpd, AtkSpeedLv); }
    public bool EnhanceCritChance() { return Enhance((int)ConstValue.Enhances.CritChance, CritChanceLv); }
    public bool EnhanceCritDamage() { return Enhance((int)ConstValue.Enhances.CritDmg, CritDamageLv); }
    public bool EnhanceGoldUp() { return Enhance((int)ConstValue.Enhances.GoldUp, GoldUpLv); }
    #endregion

    #region 무기
    public bool BuyWeapon()
    {
        WeaponData wData = Managers.Data.GetWeaponData(WeaponLv);
        if (wData == null)
        {
            Debug.Log($"Wrong weaponLv to buy: {WeaponLv}");
            return false;
        }

        if (Purchase(ConstValue.Goods.Gold, wData.Cost) == false)
            return false;

        WeaponLv++;
        return true;
    }
    #endregion

    #region 보물
    public bool BuyTreasure()
    {


        return true;
    }
    #endregion

    #region 상점
    public bool BuyShopGoods()
    {


        return true;
    }
    #endregion
}