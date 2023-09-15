using System;
using UnityEngine;

// ���� ������ ����� Ŭ����
public class GameData
{
    // ��ȭ ����
    public readonly int AtkPowLv;
    public readonly int AtkSpdLv;
    public readonly int CritChanceLv;
    public readonly int CritDmgLv;
    public readonly int GoldUpLv;
    // ���� ����
    public readonly int WeaponLv;
    // ���� ����
    public readonly int Treasure_AtkPowLv;
    public readonly int Treasure_AtkSpdLv;
    public readonly int Treasure_CritChanceLv;
    public readonly int Treasure_CritDmgLv;
    public readonly int Treasure_GoldUpLv;
    // ���� ������
    public readonly string NickName;
    public readonly int CurGold;
    public readonly int CurGem;
    public readonly int StageIdx;
    // ������ ����
    public readonly int LastAccessYear;
    public readonly int LastAccessDayOfYear;
    public readonly int LastAccessMinutes;
    // ���� ī��Ʈ
    public readonly int AdCount_Gold_2hr;
    public readonly int AdCount_Gem_100;
    // ���� ����
    public readonly int BossLv;

    // �̸� �����
    string[] strValues;

    #region ������
    public GameData(string nickname)
    {
        AtkPowLv = 0;
        AtkSpdLv = 0;
        CritChanceLv = 0;
        CritDmgLv = 0;
        GoldUpLv = 0;

        WeaponLv = 0;

        Treasure_AtkPowLv = 0;
        Treasure_AtkSpdLv = 0;
        Treasure_CritChanceLv = 0;
        Treasure_CritDmgLv = 0;
        Treasure_GoldUpLv = 0;

        NickName = nickname;
        CurGold = 0;
        CurGem = 0;
        StageIdx = 0;

        LastAccessYear = -1;
        LastAccessDayOfYear = -1;
        LastAccessMinutes = -1;

        AdCount_Gold_2hr = 0;
        AdCount_Gem_100 = 0;

        BossLv = 0;

        SaveStringValues();
    }

    public GameData(int atkPowLv, int atkSpdLv, int critChanceLv, int critDmgLv, int goldUpLv, int weaponLv,
                    int tr_atkPowLv, int tr_atkSpdLv, int tr_critChanceLv, int tr_critDmgLv, int tr_goldUpLv,
                    string nickname, int curGold, int curGem, int stageIdx,
                    int lastYear, int lastDayOfYear, int lastMinutes, int adCount_gold2hr, int adCount_gem100, int bossLv)
    {
        AtkPowLv = atkPowLv;
        AtkSpdLv = atkSpdLv;
        CritChanceLv = critChanceLv;
        CritDmgLv = critDmgLv;
        GoldUpLv = goldUpLv;

        WeaponLv = weaponLv;

        Treasure_AtkPowLv = tr_atkPowLv;
        Treasure_AtkSpdLv = tr_atkSpdLv;
        Treasure_CritChanceLv = tr_critChanceLv;
        Treasure_CritDmgLv = tr_critDmgLv;
        Treasure_GoldUpLv = tr_goldUpLv;

        NickName = nickname;
        CurGold = curGold; 
        CurGem = curGem;
        StageIdx = stageIdx;

        LastAccessYear = lastYear;
        LastAccessDayOfYear = lastDayOfYear;
        LastAccessMinutes = lastMinutes;

        AdCount_Gold_2hr = adCount_gold2hr;
        AdCount_Gem_100 = adCount_gem100;

        BossLv = bossLv;

        SaveStringValues();
    }
    #endregion

    #region ���ڿ� �� ���� / ��ȯ
    void SaveStringValues()
    {
        strValues = new string[Enum.GetNames(typeof(ConstValue.GameDataVal)).Length];
        // ��ȭ
        strValues[(int)ConstValue.GameDataVal.AtkPowLv] = AtkPowLv.ToString();
        strValues[(int)ConstValue.GameDataVal.AtkSpdLv] = AtkSpdLv.ToString();
        strValues[(int)ConstValue.GameDataVal.CritChanceLv] = CritChanceLv.ToString();
        strValues[(int)ConstValue.GameDataVal.CritDmgLv] = CritDmgLv.ToString();
        strValues[(int)ConstValue.GameDataVal.GoldUpLv] = GoldUpLv.ToString();
        // ����
        strValues[(int)ConstValue.GameDataVal.WeaponLv] = WeaponLv.ToString();
        // ����
        strValues[(int)ConstValue.GameDataVal.Treasure_AtkPowLv] = Treasure_AtkPowLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_AtkSpdLv] = Treasure_AtkSpdLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_CritChanceLv] = Treasure_CritChanceLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_CritDmgLv] = Treasure_CritDmgLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_GoldUpLv] = Treasure_GoldUpLv.ToString();
        // ���� ������
        strValues[(int)ConstValue.GameDataVal.NickName] = NickName;
        strValues[(int)ConstValue.GameDataVal.CurGold] = CurGold.ToString();
        strValues[(int)ConstValue.GameDataVal.CurGem] = CurGem.ToString();
        strValues[(int)ConstValue.GameDataVal.StageIdx] = StageIdx.ToString();
        // ���� �ð�
        strValues[(int)ConstValue.GameDataVal.LastAccessYear] = LastAccessYear.ToString();
        strValues[(int)ConstValue.GameDataVal.LastAccessDayOfYear] = LastAccessDayOfYear.ToString();
        strValues[(int)ConstValue.GameDataVal.LastAccessMinutes] = LastAccessMinutes.ToString();
        // ���� ī��Ʈ
        strValues[(int)ConstValue.GameDataVal.AdCount_Gold_2hr] = AdCount_Gold_2hr.ToString();
        strValues[(int)ConstValue.GameDataVal.AdCount_Gem_100] = AdCount_Gem_100.ToString();
        // ���� ����
        strValues[(int)ConstValue.GameDataVal.BossLv] = BossLv.ToString();
    }

    public string GetStringValue(int idx)
    {
        if(idx < 0 || idx >= strValues.Length)
        {
            Debug.Log($"Wrong GameData Idx: {idx}");
            return null;
        }

        return strValues[idx];
    }
    #endregion
}