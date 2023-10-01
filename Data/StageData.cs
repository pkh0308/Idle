// 스테이지 데이터 저장용
using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    public int StageIdx;
    public int Hp;
    public int EnemyCount;
    public int MinEnemyId;
    public int MaxEnemyId;
    public int DropGold;
    public int DropGem;
}

[Serializable]
public class StageDataLoader : ILoader<int, StageData>
{
    public List<StageData> StageDatas = new List<StageData>();

    public Dictionary<int, StageData> MakeDic()
    {
        Dictionary<int, StageData> dic = new Dictionary<int, StageData>();
        foreach (StageData data in StageDatas)
            dic.Add(data.StageIdx, data);

        return dic;
    }
}