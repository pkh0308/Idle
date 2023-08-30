using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class DataManager 
{
    const string Root = "root";
    const string GameDataPath = "/XML/GameDatas.xml";
    const string StageDataPath = "/XML/StageDatas.xml";
    const string EnhanceDataPath = "/XML/EnhanceDatas.xml";
    const string WeaponDataPath = "/XML/WeaponDatas.xml";
    const string TreasureDataPath = "/XML/TreasureDatas.xml";
    const string ShopDataPath = "/XML/ShopDatas.xml";

    public GameData CurGameData { get; private set; }

    Dictionary<int, StageData> _stageDatas = new Dictionary<int, StageData>();
    Dictionary<int, EnhanceData> _enhanceDatas = new Dictionary<int, EnhanceData>();
    Dictionary<int, WeaponData> _weaponDatas = new Dictionary<int, WeaponData>();
    Dictionary<int, TreasureData> _treasureDatas = new Dictionary<int, TreasureData>();
    Dictionary<int, ShopData> _shopDatas = new Dictionary<int, ShopData>();

    public void Init()
    {
        LoadGameData();
        LoadStageData();
        LoadEnhanceData();
        LoadWeaponData();
        LoadTreasureeData();
        LoadShopData();
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
            int atkPowLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkPowLv.ToString()));
            int atkSpeedLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkSpdLv.ToString()));
            int critChanceLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritChanceLv.ToString()));
            int critDmgLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritDmgLv.ToString()));
            int goldUpLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.GoldUpLv.ToString()));

            int weaponLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.GoldUpLv.ToString()));

            int tr_atkPowLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_AtkPowLv.ToString()));
            int tr_atkSpeedLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_AtkSpdLv.ToString()));
            int tr_critChanceLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_CritChanceLv.ToString()));
            int tr_critDmgLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_CritDmgLv.ToString()));
            int tr_goldUpLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.Treasure_GoldUpLv.ToString()));

            string nickname = node.GetAttribute(ConstValue.GameDataVal.NickName.ToString());
            int curGold = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CurGold.ToString()));
            int curGem = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CurGem.ToString()));
            int stageIdx = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.StageIdx.ToString()));

            //가져온 데이터 저장
            GameData data = new GameData(atkPowLv, atkSpeedLv, critChanceLv, critDmgLv, goldUpLv, weaponLv,
                                         tr_atkPowLv, tr_atkSpeedLv, tr_critChanceLv, tr_critDmgLv, tr_goldUpLv, 
                                         nickname, curGold, curGem, stageIdx);
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
        string[] names = System.Enum.GetNames(typeof(ConstValue.GameDataVal));
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
            int hp = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.Hp.ToString()));
            int enemyCount = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.EnemyCount.ToString()));
            int minEnemyId = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.MinEnemyId.ToString()));
            int maxEnemyId = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.MaxEnemyId.ToString()));
            int dropGold = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.DropGold.ToString()));
            int dropGem = System.Convert.ToInt32(node.GetAttribute(ConstValue.StageDataVal.DropGem.ToString()));

            //가져온 데이터 저장
            StageData data = new StageData(hp, enemyCount, minEnemyId, maxEnemyId, dropGold, dropGem);
            _stageDatas.Add(idx, data);
            idx++;
        }
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
            int atkPowCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkPowCost.ToString()));
            int atkSpdCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkSpdCost.ToString()));
            int critChanceCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritChanceCost.ToString()));
            int critDmgCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritDmgCost.ToString()));
            int goldUpCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.GoldUpCost.ToString()));

            int atkPowValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkPowValue.ToString()));
            int atkSpdValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.AtkSpdValue.ToString()));
            int critChanceValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritChanceValue.ToString()));
            int critDmgValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.CritDmgValue.ToString()));
            int goldUpValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.EnhanceDataVal.GoldUpValue.ToString()));

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
            int atkPow = System.Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.AtkPower.ToString()));
            int critChance = System.Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.CritChance.ToString()));
            int critDamage = System.Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.CritDamage.ToString()));
            int cost = System.Convert.ToInt32(node.GetAttribute(ConstValue.WeaponDataVal.Cost.ToString()));

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
            int atkPowCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkPowCost.ToString()));
            int atkSpdCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkSpdCost.ToString()));
            int critChanceCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritChanceCost.ToString()));
            int critDmgCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritDmgCost.ToString()));
            int goldUpCost = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_GoldUpCost.ToString()));

            int atkPowValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkPowValue.ToString()));
            int atkSpdValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_AtkSpdValue.ToString()));
            int critChanceValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritChanceValue.ToString()));
            int critDmgValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_CritDmgValue.ToString()));
            int goldUpValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.TreasureDataVal.Tr_GoldUpValue.ToString()));

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
            int maxCount = System.Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.MaxCount.ToString()));
            int goodsType = System.Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.GoodsType.ToString()));
            int goodsValue = System.Convert.ToInt32(node.GetAttribute(ConstValue.ShopDataVal.GoodsValue.ToString()));

            //가져온 데이터 저장
            ShopData data = new ShopData(name, maxCount, goodsType, goodsValue);
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
}