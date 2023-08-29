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

    public GameData CurGameData { get; private set; }

    Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();
    Dictionary<int, EnhanceData> enhanceDatas = new Dictionary<int, EnhanceData>();
    Dictionary<int, WeaponData> weaponDatas = new Dictionary<int, WeaponData>();
    Dictionary<int, TreasureData[]> treasureDatas = new Dictionary<int, TreasureData[]>();
    Dictionary<int, ShopData> shopDatas = new Dictionary<int, ShopData>();

    public void Init()
    {
        LoadGameData();
        LoadStageData();
        LoadEnhanceData();
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
            stageDatas.Add(idx, data);
            idx++;
        }
    }

    public StageData GetStageData(int stageIdx)
    {
        if (stageDatas.TryGetValue(stageIdx, out StageData sData) == false)
        {
            Debug.Log($"Wrong Stage Idx: {stageIdx}");
            return null;
        }
        return sData;
    }

    public int GetStageGold(int stageIdx)
    {
        if(stageDatas.TryGetValue(stageIdx, out StageData sData) == false)
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
            enhanceDatas.Add(level, data);
            level++;
        }
    }

    int GetEnahnceData(int idx, int level, bool isCost)
    {
        if (enhanceDatas.TryGetValue(level, out EnhanceData data) == false)
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
}