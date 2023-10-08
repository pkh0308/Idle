using System;
using System.Collections.Generic;

// ���� ������ ����� Ŭ����
[Serializable]
public class GameData
{
    // ��ȭ ����
    public int AtkPowLv;
    public int AtkSpdLv;
    public int CritChanceLv;
    public int CritDmgLv;
    public int GoldUpLv;
    // ���� ����
    public int WeaponLv;
    // ���� ����
    public int Treasure_AtkPowLv;
    public int Treasure_AtkSpdLv;
    public int Treasure_CritChanceLv;
    public int Treasure_CritDmgLv;
    public int Treasure_GoldUpLv;
    // ���� ������
    public string NickName;
    public long CurGold;
    public int CurGem;
    public int StageIdx;
    // ������ ����
    public int LastAccessYear;
    public int LastAccessDayOfYear;
    public int LastAccessMinutes;
    // ���� ī��Ʈ
    public int AdCount_Gold_2hr;
    public int AdCount_Gem_100;
    // ���� ����
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