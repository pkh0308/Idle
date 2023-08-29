using UnityEngine;

public class ConstValue
{
    public const string EventSystem = "EventSystem";

    // notice
    public const string Notice_LessThan2 = "�г����� 2�� �̻��̾�� �մϴ�.";
    public const string Notice_NoBlankInName = "�г����� ������ ������ �� �����ϴ�.";
    public const string Notice_NoSaveData = "����� �����Ͱ� �����ϴ�.";
    public const string Notice_NotEnoughMoney = "��ȭ�� �����մϴ�.";

    public const int NumOfEnhance = 5;

    #region ������
    public enum GameDataVal
    {
        AtkPowLv,
        AtkSpdLv,
        CritChanceLv,
        CritDmgLv,
        GoldUpLv,
        WeaponLv,
        Treasure_AtkPowLv,
        Treasure_AtkSpdLv,
        Treasure_CritChanceLv,
        Treasure_CritDmgLv,
        Treasure_GoldUpLv,
        NickName,
        CurGold,
        CurGem,
        StageIdx
    }

    public enum StageDataVal
    {
        Hp,
        EnemyCount,
        MinEnemyId,
        MaxEnemyId,
        DropGold,
        DropGem
    }

    public enum EnhanceDataVal
    {
        AtkPowCost,
        AtkSpdCost,
        CritChanceCost,
        CritDmgCost,
        GoldUpCost,
        AtkPowValue,
        AtkSpdValue,
        CritChanceValue,
        CritDmgValue,
        GoldUpValue
    }

    public enum Enhances
    {
        AtkPow,
        AtkSpd,
        CritChance,
        CritDmg,
        GoldUp
    }
    #endregion
}