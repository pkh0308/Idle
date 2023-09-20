using UnityEngine;

public class ConstValue
{
    public const string EventSystem = "EventSystem";
    public const string Max = "Max";
    public const string Enemy = "Enemy_";
    public const string Sprite_EnemyDie = "Sprite_EnemyDie_";
    public const string Boss = "Boss_";
    public const string Sprite_BossDie = "Sprite_BossDie_";
    public const string DmgText = "DamageText";

    // notice
    public const string Notice_LessThan2 = "닉네임은 2자 이상이어야 합니다.";
    public const string Notice_NoBlankInName = "닉네임은 공백을 포함할 수 없습니다.";
    public const string Notice_NoSaveData = "저장된 데이터가 없습니다.";
    public const string Notice_NotEnoughMoney = "재화가 부족합니다.";
    public const string Notice_NotSelectBoss = "보스를 선택해주세요.";
    public const string Notice_AdReward = "광고 보상을 획득했습니다.";
    public const string Notice_AdNotPrepared = "광고 준비중입니다.\n잠시만 기다려주세요.";
    public const string Notice_AdCountOver = "시청 횟수를 모두 소진하였습니다.";

    public const float StageChangeTime = 2.0f;
    public const float CheerBuffRate = 2.0f;

    public const int NumOfEnhances = 5;
    public const int NumOfTreasures = 5;
    public const int MaxFrame = 60;
    public const int MaxAttackDelay = 6000;
    public const int CheerUpValue = 3;

    public const int Days = 365;
    public const int Hours = 24;
    public const int Minutes = 60;

    #region 열거형
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
        LastAccessMinutes,
        AdCount_Gold_2hr,
        AdCount_Gem_100,
        BossLv
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
    public enum BossDataVal
    {
        Hp,
        BossId,
        BossName,
        TimeLimit,
        DropGold,
        DropGem
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

    public enum Ads
    {
        Gold_2hr,
        Gem_100
    }

    public enum Goods
    {
        Gold,
        Gem
    }
    #endregion
}