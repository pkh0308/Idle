// 강화 데이터 저장용
using System;
using System.Collections.Generic;

[Serializable]
public class EnhanceData
{
    public int EnhanceLevel;

    public int AtkPowCost;
    public int AtkSpdCost;
    public int CritChanceCost;
    public int CritDmgCost;
    public int GoldUpCost;

    public int AtkPowValue;
    public int AtkSpdValue;
    public int CritChanceValue;
    public int CritDmgValue;
    public int GoldUpValue;
}

[Serializable]
public class EnhanceDataLoader : ILoader<int, EnhanceData>
{
    public List<EnhanceData> EnhanceDatas = new List<EnhanceData>();

    public Dictionary<int, EnhanceData> MakeDic()
    {
        Dictionary<int, EnhanceData> dic = new Dictionary<int, EnhanceData>();
        foreach (EnhanceData data in EnhanceDatas)
            dic.Add(data.EnhanceLevel, data);

        return dic;
    }
}