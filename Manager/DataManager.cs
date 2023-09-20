using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System;

public class DataManager 
{
    const string Root = "root";
    const string GameDataPath = "/XML/GameDatas.xml";
    const string StageDataPath = "/XML/StageDatas.xml";
    const string EnhanceDataPath = "/XML/EnhanceDatas.xml";
    const string WeaponDataPath = "/XML/WeaponDatas.xml";
    const string TreasureDataPath = "/XML/TreasureDatas.xml";
    const string ShopDataPath = "/XML/ShopDatas.xml";
    const string BossDataPath = "/XML/BossDatas.xml";

    public GameData CurGameData { get; private set; }

    Dictionary<int, StageData> _stageDatas = new Dictionary<int, StageData>();
    Dictionary<int, EnhanceData> _enhanceDatas = new Dictionary<int, EnhanceData>();
    Dictionary<int, WeaponData> _weaponDatas = new Dictionary<int, WeaponData>();
    Dictionary<int, TreasureData> _treasureDatas = new Dictionary<int, TreasureData>();
    Dictionary<int, ShopData> _shopDatas = new Dictionary<int, ShopData>();
    Dictionary<int, BossData> _bossDatas = new Dictionary<int, BossData>();

    public void Init()
    {
        LoadGameData();
        LoadStageData();
        LoadEnhanceData();
        LoadWeaponData();
        LoadTreasureeData();
        LoadShopData();
        LoadBossData();
    }

    #region GameData
    public void LoadGameData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + GameDataPath;
        if (File.Exists(path) == false)
        {
            CurGameData = null;
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];

        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int atkPowLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkPowLv.ToString()));
            int atkSpeedLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkSpdLv.ToString()));
            int critChanceLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritChanceLv.ToString()));
            int critDmgLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritDmgLv.ToString()));
            int goldUpLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.GoldUpLv.ToString()));

            int weaponLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.WeaponLv.ToString()));

            int tr_atkPowLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_AtkPowLv.ToString()));
            int tr_atkSpeedLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_AtkSpdLv.ToString()));
            int tr_critChanceLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_CritChanceLv.ToString()));
            int tr_critDmgLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_CritDmgLv.ToString()));
            int tr_goldUpLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_GoldUpLv.ToString()));

            string nickname = node.GetAttribute(ConstValue.GameDataVal.NickName.ToString());
            int curGold = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CurGold.ToString()));
            int curGem = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CurGem.ToString()));
            int stageIdx = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.StageIdx.ToString()));

            int lastYear = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.LastAccessYear.ToString()));
            int lastDayOfYear = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.LastAccessDayOfYear.ToString()));
            int lastMins = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.LastAccessMinutes.ToString()));

            int adCnt_Gold_2hr = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AdCount_Gold_2hr.ToString()));
            int adCnt_Gem_100 = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AdCount_Gem_100.ToString()));

            int bossLv = Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.BossLv.ToString()));

            //가져온 데이터 저장
            GameData data = new GameData(atkPowLv, atkSpeedLv, critChanceLv, critDmgLv, goldUpLv, weaponLv,
                                         tr_atkPowLv, tr_atkSpeedLv, tr_critChanceLv, tr_critDmgLv, tr_goldUpLv, 
                                         nickname, curGold, curGem, stageIdx, lastYear, lastDayOfYear, lastMins,
                                         adCnt_Gold_2hr, adCnt_Gem_100, bossLv);
            CurGameData = data;
        }
    }

    public void SaveGameData()
    {
        if (CurGameData == null)
        {
            Debug.Log("There is no GameData");
            return;
        }

        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement(Root);
        doc.AppendChild(root);

        XmlElement element = doc.CreateElement("Data");
        string[] names = Enum.GetNames(typeof(ConstValue.GameDataVal));
        for (int i = 0; i < names.Length; i++)
            element.SetAttribute(names[i], CurGameData.GetStringValue(i));
        root.AppendChild(element);
        
        doc.Save(Application.dataPath + GameDataPath);
    }

    // 신규 게임데이터 생성
    public void CreateNewGameData(string nickname)
    {
        GameData gameData = new GameData(nickname);
        CurGameData = gameData;
    }

    public void SetGameData(GameData data)
    {
        CurGameData = data;
    }
    #endregion

    #region StageData
    public int MaxStageIdx { get; private set; }
    void LoadStageData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + StageDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no StageDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int idx = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int hp = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.Hp.ToString()));
            int enemyCount = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.EnemyCount.ToString()));
            int minEnemyId = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.MinEnemyId.ToString()));
            int maxEnemyId = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.MaxEnemyId.ToString()));
            int dropGold = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.DropGold.ToString()));
            int dropGem = Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.DropGem.ToString()));

            //가져온 데이터 저장
            StageData data = new StageData(hp, enemyCount, minEnemyId, maxEnemyId, dropGold, dropGem);
            _stageDatas.Add(idx, data);
            idx++;
        }
        MaxStageIdx = _stageDatas.Count - 1;
    }

    public StageData GetStageData(int stageIdx)
    {
        if (_stageDatas.TryGetValue(stageIdx, out StageData sData) == false)
        {
            Debug.Log($"Wrong Stage Idx: {stageIdx}");
            return null;
        }
        return sData;
    }

    public int GetStageGold(int stageIdx)
    {
        if(_stageDatas.TryGetValue(stageIdx, out StageData sData) == false)
        {
            Debug.Log($"Wrong Stage Idx: {stageIdx}");
            return 0;
        }
        return sData.DropGold;
    }
    #endregion

    #region EnhanceData
    void LoadEnhanceData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + EnhanceDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no EnhanceDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int level = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int atkPowCost = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkPowCost.ToString()));
            int atkSpdCost = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkSpdCost.ToString()));
            int critChanceCost = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritChanceCost.ToString()));
            int critDmgCost = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritDmgCost.ToString()));
            int goldUpCost = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.GoldUpCost.ToString()));

            int atkPowValue = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkPowValue.ToString()));
            int atkSpdValue = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkSpdValue.ToString()));
            int critChanceValue = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritChanceValue.ToString()));
            int critDmgValue = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritDmgValue.ToString()));
            int goldUpValue = Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.GoldUpValue.ToString()));

            //가져온 데이터 저장
            EnhanceData data = new EnhanceData(atkPowCost, atkSpdCost, critChanceCost, critDmgCost, goldUpCost,
                                               atkPowValue, atkSpdValue, critChanceValue, critDmgValue, goldUpValue);
            _enhanceDatas.Add(level, data);
            level++;
        }
    }

    int GetEnahnceData(int idx, int level, bool isCost)
    {
        if (_enhanceDatas.TryGetValue(level, out EnhanceData data) == false)
        {
            Debug.Log($"Wrong level for Enhance: {level}");
            return -1;
        }

        if(isCost)
            return data.GetCost(idx);
        else
            return data.GetValue(idx);
    }

    public int GetEnahnceCost(int idx, int level)  { return GetEnahnceData(idx, level, true); }
    public int GetEnahnceValue(int idx, int level) { return GetEnahnceData(idx, level, false); }
    #endregion

    #region WeaponData
    void LoadWeaponData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + WeaponDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no WeaponDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int level = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int atkPow = Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.AtkPower.ToString()));
            int critChance = Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.CritChance.ToString()));
            int critDamage = Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.CritDamage.ToString()));
            int cost = Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.Cost.ToString()));

            //가져온 데이터 저장
            WeaponData data = new WeaponData(atkPow, critChance, critDamage, cost);
            _weaponDatas.Add(level, data);
            level++;
        }
    }

    public WeaponData GetWeaponData(int weaponLv)
    {
        if (_weaponDatas.TryGetValue(weaponLv, out WeaponData data) == false)
        {
            Debug.Log($"Wrong WeaponLv to buy: {weaponLv}");
            return null;
        }
        return data;
    }
    #endregion

    #region TreasureData
    void LoadTreasureeData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + TreasureDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no TreasureDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int level = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int atkPowCost = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkPowCost.ToString()));
            int atkSpdCost = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkSpdCost.ToString()));
            int critChanceCost = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritChanceCost.ToString()));
            int critDmgCost = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritDmgCost.ToString()));
            int goldUpCost = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_GoldUpCost.ToString()));

            int atkPowValue = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkPowValue.ToString()));
            int atkSpdValue = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkSpdValue.ToString()));
            int critChanceValue = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritChanceValue.ToString()));
            int critDmgValue = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritDmgValue.ToString()));
            int goldUpValue = Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_GoldUpValue.ToString()));

            //가져온 데이터 저장
            TreasureData data = new TreasureData(atkPowCost, atkSpdCost, critChanceCost, critDmgCost, goldUpCost,
                                                 atkPowValue, atkSpdValue, critChanceValue, critDmgValue, goldUpValue);
            _treasureDatas.Add(level, data);
            level++;
        }
    }

    int GetTreasureData(int idx, int level, bool isCost)
    {
        if (_treasureDatas.TryGetValue(level, out TreasureData data) == false)
        {
            Debug.Log($"Wrong level for Treasure: {level}");
            return -1;
        }

        return isCost ? data.GetCost(idx) : data.GetValue(idx);
    }

    public int GetTreasureCost(int idx, int level) { return GetTreasureData(idx, level, true); }
    public int GetTreasureValue(int idx, int level) { return GetTreasureData(idx, level, false); }

    string GetTreasureByString(int level, int idx = -1)
    {
        if (_treasureDatas.TryGetValue(level, out TreasureData data) == false)
        {
            Debug.Log($"Wrong level for Treasure: {level}");
            return null;
        }

        // 최고 레벨일 경우
        if (_treasureDatas.TryGetValue(level + 1, out TreasureData nextData) == false)
            return idx < 0 ? ConstValue.Max : $"{data.GetValue(idx) / 100}%";

        // 최고 레벨이 아닐 경우
        // 다음 레벨 정보 노출
        return idx < 0 ? (level + 1).ToString() : $"{nextData.GetValue(idx) / 100}%";
    }

    public string GetTrLevelByStr(int level) { return GetTreasureByString(level); }
    public string GetTrValueByStr(int level, int idx) { return GetTreasureByString(level, idx); }
    #endregion

    #region ShopData
    void LoadShopData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + ShopDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no ShopDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int idx = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            string name = node.GetAttribute(ConstValue.ShopDataVal.Name.ToString());
            int cost = Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.Cost.ToString()));
            int maxCount = Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.MaxCount.ToString()));
            int goodsType = Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.GoodsType.ToString()));
            int goodsValue = Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.GoodsValue.ToString()));

            //가져온 데이터 저장
            ShopData data = new ShopData(name, cost, maxCount, goodsType, goodsValue);
            _shopDatas.Add(idx, data);
            idx++;
        }
    }

    public ShopData GetShopData(int idx)
    {
        if(_shopDatas.TryGetValue(idx, out ShopData data) == false)
        {
            Debug.Log($"Wrong idx to buy: {idx}");
            return null;
        }
        return data;
    }
    #endregion

    #region BossData
    void LoadBossData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + BossDataPath;
        if (File.Exists(path) == false)
        {
            Debug.Log($"There is no BossDatas At {path}");
            return;
        }

        // 루트 설정
        doc.Load(path);
        XmlElement nodes = doc[Root];
        int idx = 0;
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int hp = Convert.ToInt32(node.GetAttribute(ConstValue.BossDataVal.Hp.ToString()));
            int id = Convert.ToInt32(node.GetAttribute(ConstValue.BossDataVal.BossId.ToString()));
            string name = node.GetAttribute(ConstValue.BossDataVal.BossName.ToString());
            int timeLimit = Convert.ToInt32(node.GetAttribute(ConstValue.BossDataVal.TimeLimit.ToString()));
            int dropGold = Convert.ToInt32(node.GetAttribute(ConstValue.BossDataVal.DropGold.ToString()));
            int dropGem = Convert.ToInt32(node.GetAttribute(ConstValue.BossDataVal.DropGem.ToString()));

            //가져온 데이터 저장
            BossData data = new BossData(hp, id, name, timeLimit, dropGold, dropGem);
            _bossDatas.Add(idx, data);
            idx++;
        }
    }

    public BossData GetBossData(int bossIdx)
    {
        if (_bossDatas.TryGetValue(bossIdx, out BossData bData) == false)
        {
            Debug.Log($"Wrong Boss Idx: {bossIdx}");
            return null;
        }
        return bData;
    }
    #endregion
}