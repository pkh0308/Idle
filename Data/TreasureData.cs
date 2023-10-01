// 보물 데이터 저장용
using System;
using System.Collections.Generic;

[Serializable]
public class TreasureData
{
    public int TreasureLevel;

    public int Tr_AtkPowCost;
    public int Tr_AtkSpdCost;
    public int Tr_CritChanceCost;
    public int Tr_CritDmgCost;
    public int Tr_GoldUpCost;

    public int Tr_AtkPowValue;
    public int Tr_AtkSpdValue;
    public int Tr_CritChanceValue;
    public int Tr_CritDmgValue;
    public int Tr_GoldUpValue;
}

[Serializable]
public class TreasureDataLoader : ILoader<int, TreasureData>
{
    public List<TreasureData> TreasureDatas = new List<TreasureData>();

    public Dictionary<int, TreasureData> MakeDic()
    {
        Dictionary<int, TreasureData> dic = new Dictionary<int, TreasureData>();
        foreach (TreasureData data in TreasureDatas)
            dic.Add(data.TreasureLevel, data);

        return dic;
    }
}
