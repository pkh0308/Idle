using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

public class DataManager 
{
    const string GameDataPath = "/GameData.json";

    const string StageDataPath = "Data/StageDatas";
    const string EnhanceDataPath = "Data/EnhanceDatas";
    const string WeaponDataPath = "Data/WeaponDatas";
    const string TreasureDataPath = "Data/TreasureDatas";
    const string ShopDataPath = "Data/ShopDatas";
    const string BossDataPath = "Data/BossDatas";

    public GameData CurGameData { get; private set; }

    public Dictionary<int, StageData> StageDatas { get; private set; } = new Dictionary<int, StageData>();
    public Dictionary<int, EnhanceData> EnhanceDatas { get; private set; } = new Dictionary<int, EnhanceData>();
    public Dictionary<int, WeaponData> WeaponDatas { get; private set; } = new Dictionary<int, WeaponData>();
    public Dictionary<int, TreasureData> TreasureDatas { get; private set; } = new Dictionary<int, TreasureData>();
    public Dictionary<int, ShopData> ShopDatas { get; private set; } = new Dictionary<int, ShopData>();
    public Dictionary<int, BossData> BossDatas { get; private set; } = new Dictionary<int, BossData>();

    public int MaxStageIdx { get; private set; }

    #region 초기화
    // xml -> json 변환용 함수
    void XmlToJson(string xmlPath, string jsonPath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlPath);
        string jsonString = JsonConvert.SerializeXmlNode(doc);

        FileStream fileStream = new FileStream(jsonPath, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonString);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void Init()
    {
        CurGameData = LoadSingleData<GameData>(GameDataPath);

        StageDatas = LoadDatas<StageDataLoader, int, StageData>(StageDataPath).MakeDic();
        EnhanceDatas = LoadDatas<EnhanceDataLoader, int, EnhanceData>(EnhanceDataPath).MakeDic(); 
        WeaponDatas = LoadDatas<WeaponDataLoader, int, WeaponData>(WeaponDataPath).MakeDic(); 
        TreasureDatas = LoadDatas<TreasureDataLoader, int, TreasureData>(TreasureDataPath).MakeDic();
        ShopDatas = LoadDatas<ShopDataLoader, int, ShopData>(ShopDataPath).MakeDic(); 
        BossDatas = LoadDatas<BossDataLoader, int, BossData>(BossDataPath).MakeDic();

        MaxStageIdx = StageDatas.Count;
    }

    Data LoadSingleData<Data>(string path)
    {
        if(File.Exists(Application.persistentDataPath + path) == false)
        {
            Debug.Log("### There is no GameData");
            return default(Data);
        }
        string dataString = File.ReadAllText(Application.persistentDataPath + path);
        return JsonUtility.FromJson<Data>(dataString);
    }

    Loader LoadDatas<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
    #endregion

    #region GameData
    public void SaveGameData()
    {
        if (CurGameData == null)
            return;

        string jsonString = JsonUtility.ToJson(CurGameData);

        FileStream fileStream = new FileStream(Application.persistentDataPath + GameDataPath, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonString);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    // 신규 게임데이터 생성
    public void CreateNewGameData(string nickname)
    {
        GameData gameData = new GameData()
        {
            AtkPowLv = 0,
            AtkSpdLv = 0,
            CritChanceLv = 0,
            CritDmgLv = 0,
            GoldUpLv = 0,

            WeaponLv = 0,

            Treasure_AtkPowLv = 0,
            Treasure_AtkSpdLv = 0, 
            Treasure_CritChanceLv = 0,
            Treasure_CritDmgLv = 0,
            Treasure_GoldUpLv = 0,

            NickName = nickname,

            CurGold = 0,
            CurGem = 0,
            StageIdx = 0,
            LastAccessYear = -1,
            LastAccessDayOfYear = -1,
            LastAccessMinutes = -1,
            AdCount_Gold_2hr = 0,
            AdCount_Gem_100 = 0,

            BossLv = 0,
        };
        CurGameData = gameData;
    }

    public void SetGameData(GameData data)
    {
        CurGameData = data;
    }
    #endregion

    #region StageData
    public StageData GetStageData(int stageIdx)
    {
        if (StageDatas.TryGetValue(stageIdx, out StageData sData) == false)
        {
            Debug.Log($"### Wrong Stage Idx: {stageIdx}");
            return null;
        }
        return sData;
    }

    public int GetStageGold(int stageIdx)
    {
        if(StageDatas.TryGetValue(stageIdx, out StageData sData) == false)
        {
            Debug.Log($"### Wrong Stage Idx: {stageIdx}");
            return 0;
        }
        return sData.DropGold;
    }
    #endregion

    #region EnhanceData
    int GetEnahnceData(int idx, int level, bool isCost)
    {
        if (EnhanceDatas.TryGetValue(level, out EnhanceData data) == false)
        {
            Debug.Log($"### Wrong Enhance Level: {level}");
            return -1;
        }

        int result = -1;
        switch(idx)
        {
            case (int)ConstValue.Enhances.AtkPow:
                result = isCost ? data.AtkPowCost : data.AtkPowValue;
                break;
            case (int)ConstValue.Enhances.AtkSpd:
                result = isCost ? data.AtkSpdCost : data.AtkSpdValue;
                break;
            case (int)ConstValue.Enhances.CritChance:
                result = isCost ? data.CritChanceCost : data.CritChanceValue;
                break;
            case (int)ConstValue.Enhances.CritDmg:
                result = isCost ? data.CritDmgCost : data.CritDmgValue;
                break;
            case (int)ConstValue.Enhances.GoldUp:
                result = isCost ? data.GoldUpCost : data.GoldUpValue;
                break;
        }
        return result;
    }

    public int GetEnahnceCost(int idx, int level)  { return GetEnahnceData(idx, level, true); }
    public int GetEnahnceValue(int idx, int level) { return GetEnahnceData(idx, level, false); }
    #endregion

    #region WeaponData
    public WeaponData GetWeaponData(int weaponLv)
    {
        if (WeaponDatas.TryGetValue(weaponLv, out WeaponData data) == false)
        {
            Debug.Log($"### Wrong WeaponLv to buy: {weaponLv}");
            return null;
        }
        return data;
    }
    #endregion

    #region TreasureData
    int GetTreasureData(int idx, int level, bool isCost)
    {
        if (TreasureDatas.TryGetValue(level, out TreasureData data) == false)
        {
            Debug.Log($"### Wrong level for Treasure: {level}");
            return -1;
        }

        int result = -1;
        switch (idx)
        {
            case (int)ConstValue.Treasures.Tr_AtkPow:
                result = isCost ? data.Tr_AtkPowCost : data.Tr_AtkPowValue;
                break;
            case (int)ConstValue.Treasures.Tr_AtkSpd:
                result = isCost ? data.Tr_AtkSpdCost : data.Tr_AtkSpdValue;
                break;
            case (int)ConstValue.Treasures.Tr_CritChance:
                result = isCost ? data.Tr_CritChanceCost : data.Tr_CritChanceValue;
                break;
            case (int)ConstValue.Treasures.Tr_CritDmg:
                result = isCost ? data.Tr_CritDmgCost : data.Tr_CritDmgValue;
                break;
            case (int)ConstValue.Treasures.Tr_GoldUp:
                result = isCost ? data.Tr_GoldUpCost : data.Tr_GoldUpValue;
                break;
        }
        return result;
    }

    public int GetTreasureCost(int idx, int level) { return GetTreasureData(idx, level, true); }
    public int GetTreasureValue(int idx, int level) { return GetTreasureData(idx, level, false); }

    string GetTreasureByString(int level, int idx = -1)
    {
        if (TreasureDatas.TryGetValue(level, out TreasureData data) == false)
        {
            Debug.Log($"### Wrong Treasure Level: {level}");
            return null;
        }

        // 최고 레벨일 경우
        if (TreasureDatas.TryGetValue(level + 1, out TreasureData nextData) == false)
        {
            // 레벨
            if (idx < 0)
                return ConstValue.Max;
            // 수치
            switch (idx)
            {
                case (int)ConstValue.Treasures.Tr_AtkPow:
                    return $"{data.Tr_AtkPowValue / 100}%";
                case (int)ConstValue.Treasures.Tr_AtkSpd:
                    return $"{data.Tr_AtkSpdValue / 100}%";
                case (int)ConstValue.Treasures.Tr_CritChance:
                    return $"{data.Tr_CritChanceValue / 100}%";
                case (int)ConstValue.Treasures.Tr_CritDmg:
                    return $"{data.Tr_CritDmgValue / 100}%";
                case (int)ConstValue.Treasures.Tr_GoldUp:
                    return $"{data.Tr_GoldUpValue / 100}%";
                default:
                    Debug.Log($"### Wrong Treasure Idx: {idx}");
                    return null;
            }
        }

        // 최고 레벨이 아닐 경우 다음 레벨 정보 노출
        // 레벨
        if (idx < 0)
            return (level + 1).ToString();
        // 수치
        switch (idx)
        {
            case (int)ConstValue.Treasures.Tr_AtkPow:
                return $"{nextData.Tr_AtkPowValue / 100}%";
            case (int)ConstValue.Treasures.Tr_AtkSpd:
                return $"{nextData.Tr_AtkSpdValue / 100}%";
            case (int)ConstValue.Treasures.Tr_CritChance:
                return $"{nextData.Tr_CritChanceValue / 100}%";
            case (int)ConstValue.Treasures.Tr_CritDmg:
                return $"{nextData.Tr_CritDmgValue / 100}%";
            case (int)ConstValue.Treasures.Tr_GoldUp:
                return $"{nextData.Tr_GoldUpValue / 100}%";
            default:
                Debug.Log($"### Wrong Treasure Idx: {idx}");
                return null;
        }
    }

    public string GetTrLevelByStr(int level) { return GetTreasureByString(level); }
    public string GetTrValueByStr(int level, int idx) { return GetTreasureByString(level, idx); }
    #endregion

    #region ShopData
    public ShopData GetShopData(int idx)
    {
        if(ShopDatas.TryGetValue(idx, out ShopData data) == false)
        {
            Debug.Log($"### Wrong shopItem idx: {idx}");
            return null;
        }
        return data;
    }
    #endregion

    #region BossData
    public BossData GetBossData(int idx)
    {
        int[] keys = BossDatas.Keys.ToArray();

        if (BossDatas.TryGetValue(keys[idx], out BossData bData) == false)
        {
            Debug.Log($"### Wrong Boss Idx: {idx}");
            return null;
        }
        return bData;
    }
    #endregion
}