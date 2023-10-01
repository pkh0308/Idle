// 보스 데이터 저장용
using System;
using System.Collections.Generic;

[Serializable]
public class BossData
{
    public int BossId;
    public string BossName;
    public int Hp;
    public int TimeLimit;
    public int DropGold;
    public int DropGem;
}

[Serializable]
public class BossDataLoader : ILoader<int, BossData>
{
    public List<BossData> BossDatas = new List<BossData>();

    public Dictionary<int, BossData> MakeDic()
    {
        Dictionary<int, BossData> dic = new Dictionary<int, BossData>();
        foreach (BossData data in BossDatas)
            dic.Add(data.BossId, data);

        return dic;
    }
}