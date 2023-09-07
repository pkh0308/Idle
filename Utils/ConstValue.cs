using UnityEngine;

public class ConstValue
{
    public const string EventSystem = "EventSystem";
    public const string Max = "Max";
    public const string Enemy = "Enemy_";
    public const string Sprite_EnemyDie = "Sprite_EnemyDie_";

    // notice
    public const string Notice_LessThan2 = "�г����� 2�� �̻��̾�� �մϴ�.";
    public const string Notice_NoBlankInName = "�г����� ������ ������ �� �����ϴ�.";
    public const string Notice_NoSaveData = "����� �����Ͱ� �����ϴ�.";
    public const string Notice_NotEnoughMoney = "��ȭ�� �����մϴ�.";

    public const float StageChangeTime = 2.0f;

    public const int NumOfEnhances = 5;
    public const int NumOfTreasures = 5;
    public const int NumOfEnemies = 5;
    public const int MaxFrame = 60;
    public const int MaxAttackDelay = 6000;

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
        StageIdx,
        LastAccessYear,
        LastAccessDayOfYear,
        LastAccessMinutes
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
    public enum WeaponDataVal
    {
        AtkPower,
        CritChance,
        CritDamage,
        Cost
    }
    public enum TreasureDataVal
    {
        Tr_AtkPowCost,
        Tr_AtkSpdCost,
        Tr_CritChanceCost,
        Tr_CritDmgCost,
        Tr_GoldUpCost,
        Tr_AtkPowValue,
        Tr_AtkSpdValue,
        Tr_CritChanceValue,
        Tr_CritDmgValue,
        Tr_GoldUpValue
    }
    public enum ShopDataVal
    {
        Name,
        Cost,
        MaxCount,
        GoodsType,
        GoodsValue
    }

    public enum Enhances
    {
        AtkPow,
        AtkSpd,
        CritChance,
        CritDmg,
        GoldUp
    }
    public enum Treasures
    {
        Tr_AtkPow,
        Tr_AtkSpd,
        Tr_CritChance,
        Tr_CritDmg,
        Tr_GoldUp
    }


    public enum Goods
    {
        Gold,
        Gem
    }
    #endregion
}