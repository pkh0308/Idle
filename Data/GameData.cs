using System;
using System.Collections.Generic;

// 게임 데이터 저장용 클래스
[Serializable]
public class GameData
{
    // 강화 레벨
    public int AtkPowLv;
    public int AtkSpdLv;
    public int CritChanceLv;
    public int CritDmgLv;
    public int GoldUpLv;
    // 무기 레벨
    public int WeaponLv;
    // 보물 레벨
    public int Treasure_AtkPowLv;
    public int Treasure_AtkSpdLv;
    public int Treasure_CritChanceLv;
    public int Treasure_CritDmgLv;
    public int Treasure_GoldUpLv;
    // 유저 데이터
    public string NickName;
    public long CurGold;
    public int CurGem;
    public int StageIdx;
    // 마지막 접속
    public int LastAccessYear;
    public int LastAccessDayOfYear;
    public int LastAccessMinutes;
    // 광고 카운트
    public int AdCount_Gold_2hr;
    public int AdCount_Gem_100;
    // 보스 레벨
    public int BossLv;
}

[Serializable]
public class GameDataLoader : ILoader<int, GameData>
{
    List<GameData> datas = new List<GameData>();

    public Dictionary<int, GameData> MakeDic()
    {
        Dictionary<int, GameData> dic = new Dictionary<int, GameData>();
        foreach (GameData gData in datas)
            dic.Add(gData.StageIdx, gData);

        return dic;
    }
}