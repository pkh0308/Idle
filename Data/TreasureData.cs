// 보물 데이터 저장용
public class TreasureData
{
    public readonly int Tr_AtkPowCost;
    public readonly int Tr_AtkSpdCost;
    public readonly int Tr_CritChanceCost;
    public readonly int Tr_CritDmgCost;
    public readonly int Tr_GoldUpCost;
    int[] costArr = new int[ConstValue.NumOfTreasures];

    public readonly int Tr_AtkPowValue;
    public readonly int Tr_AtkSpdValue;
    public readonly int Tr_CritChanceValue;
    public readonly int Tr_CritDmgValue;
    public readonly int Tr_GoldUpValue;
    int[] valueArr = new int[ConstValue.NumOfTreasures];

    public TreasureData(int Tr_AtkPowCost, int Tr_AtkSpdCost, int Tr_CritChanceCost, int Tr_CritDmgCost, int Tr_GoldUpCost,
                        int Tr_AtkPowValue, int Tr_AtkSpdValue, int Tr_CritChanceValue, int Tr_CritDmgValue, int Tr_GoldUpValue)
    {
        this.Tr_AtkPowCost = Tr_AtkPowCost;
        this.Tr_AtkSpdCost = Tr_AtkSpdCost;
        this.Tr_CritChanceCost = Tr_CritChanceCost;
        this.Tr_CritDmgCost = Tr_CritDmgCost;
        this.Tr_GoldUpCost = Tr_GoldUpCost;

        this.Tr_AtkPowValue = Tr_AtkPowValue;
        this.Tr_AtkSpdValue = Tr_AtkSpdValue;
        this.Tr_CritChanceValue = Tr_CritChanceValue;
        this.Tr_CritDmgValue = Tr_CritDmgValue;
        this.Tr_GoldUpValue = Tr_GoldUpValue;

        BindData();
    }

    void BindData()
    {
        costArr[(int)ConstValue.Treasures.Tr_AtkPow] = Tr_AtkPowCost;
        costArr[(int)ConstValue.Treasures.Tr_AtkSpd] = Tr_AtkSpdCost;
        costArr[(int)ConstValue.Treasures.Tr_CritChance] = Tr_CritChanceCost;
        costArr[(int)ConstValue.Treasures.Tr_CritDmg] = Tr_CritDmgCost;
        costArr[(int)ConstValue.Treasures.Tr_GoldUp] = Tr_GoldUpCost;

        valueArr[(int)ConstValue.Treasures.Tr_AtkPow] = Tr_AtkPowValue;
        valueArr[(int)ConstValue.Treasures.Tr_AtkSpd] = Tr_AtkSpdValue;
        valueArr[(int)ConstValue.Treasures.Tr_CritChance] = Tr_CritChanceValue;
        valueArr[(int)ConstValue.Treasures.Tr_CritDmg] = Tr_CritDmgValue;
        valueArr[(int)ConstValue.Treasures.Tr_GoldUp] = Tr_GoldUpValue;
    }

    public int GetCost(int idx)
    {
        return costArr[idx];
    }
    public int GetValue(int idx)
    {
        return valueArr[idx];
    }
}