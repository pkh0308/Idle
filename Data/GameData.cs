using System;
using UnityEngine;

// 게임 데이터 저장용 클래스
public class GameData
{
    // 강화 레벨
    public readonly int AtkPowLv;
    public readonly int AtkSpdLv;
    public readonly int CritChanceLv;
    public readonly int CritDmgLv;
    public readonly int GoldUpLv;
    // 무기 레벨
    public readonly int WeaponLv;
    // 보물 레벨
    public readonly int Treasure_AtkPowLv;
    public readonly int Treasure_AtkSpdLv;
    public readonly int Treasure_CritChanceLv;
    public readonly int Treasure_CritDmgLv;
    public readonly int Treasure_GoldUpLv;
    // 유저 데이터
    public readonly string NickName;
    public readonly int CurGold;
    public readonly int CurGem;
    public readonly int StageIdx;

    // 이름 저장용
    string[] strValues;

    #region 생성자
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

        SaveStringValues();
    }

    public GameData(int atkPowLv, int atkSpdLv, int critChanceLv, int critDmgLv, int goldUpLv, int weaponLv,
                    int tr_atkPowLv, int tr_atkSpdLv, int tr_critChanceLv, int tr_critDmgLv, int tr_goldUpLv,
                    string nickname, int curGold, int curGem, int stageIdx)
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

        SaveStringValues();
    }

    void SaveStringValues()
    {
        strValues = new string[Enum.GetNames(typeof(ConstValue.GameDataVal)).Length];
        // 강화
        strValues[(int)ConstValue.GameDataVal.AtkPowLv] = AtkPowLv.ToString();
        strValues[(int)ConstValue.GameDataVal.AtkSpdLv] = AtkSpdLv.ToString();
        strValues[(int)ConstValue.GameDataVal.CritChanceLv] = CritChanceLv.ToString();
        strValues[(int)ConstValue.GameDataVal.CritDmgLv] = CritDmgLv.ToString();
        strValues[(int)ConstValue.GameDataVal.GoldUpLv] = GoldUpLv.ToString();
        // 웨펀
        strValues[(int)ConstValue.GameDataVal.WeaponLv] = WeaponLv.ToString();
        // 보물
        strValues[(int)ConstValue.GameDataVal.Treasure_AtkPowLv] = Treasure_AtkPowLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_AtkSpdLv] = Treasure_AtkSpdLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_CritChanceLv] = Treasure_CritChanceLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_CritDmgLv] = Treasure_CritDmgLv.ToString();
        strValues[(int)ConstValue.GameDataVal.Treasure_GoldUpLv] = Treasure_GoldUpLv.ToString();
        // 유저 데이터
        strValues[(int)ConstValue.GameDataVal.NickName] = NickName.ToString();
        strValues[(int)ConstValue.GameDataVal.CurGold] = CurGold.ToString();
        strValues[(int)ConstValue.GameDataVal.CurGem] = CurGem.ToString();
        strValues[(int)ConstValue.GameDataVal.StageIdx] = StageIdx.ToString();
    }
    #endregion

    public string GetStringValue(int idx)
    {
        if(idx < 0 || idx >= strValues.Length)
        {
            Debug.Log($"Wrong GameData Idx: {idx}");
            return null;
        }

        return strValues[idx];
    }
}