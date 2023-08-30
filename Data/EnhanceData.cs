// 강화 데이터 저장용
public class EnhanceData
{
    public readonly int AtkPowCost;
    public readonly int AtkSpdCost;
    public readonly int CritChanceCost;
    public readonly int CritDmgCost;
    public readonly int GoldUpCost;
    int[] costArr = new int[ConstValue.NumOfEnhances];

    public readonly int AtkPowValue;
    public readonly int AtkSpdValue;
    public readonly int CritChanceValue;
    public readonly int CritDmgValue;
    public readonly int GoldUpValue;
    int[] valueArr = new int[ConstValue.NumOfEnhances];

    public EnhanceData(int AtkPowCost, int AtkSpdCost, int CritChanceCost, int CritDmgCost, int GoldUpCost,
                       int AtkPowValue, int AtkSpdValue, int CritChanceValue, int CritDmgValue, int GoldUpValue)
    {
        this.AtkPowCost = AtkPowCost;
        this.AtkSpdCost = AtkSpdCost;
        this.CritChanceCost = CritChanceCost;
        this.CritDmgCost = CritDmgCost;
        this.GoldUpCost = GoldUpCost;

        this.AtkPowValue = AtkPowValue;
        this.AtkSpdValue = AtkSpdValue;
        this.CritChanceValue = CritChanceValue;
        this.CritDmgValue = CritDmgValue;
        this.GoldUpValue = GoldUpValue;

        BindData();
    }

    void BindData()
    {
        costArr[(int)ConstValue.Enhances.AtkPow] = AtkPowCost;
        costArr[(int)ConstValue.Enhances.AtkSpd] = AtkSpdCost;
        costArr[(int)ConstValue.Enhances.CritChance] = CritChanceCost;
        costArr[(int)ConstValue.Enhances.CritDmg] = CritDmgCost;
        costArr[(int)ConstValue.Enhances.GoldUp] = GoldUpCost;

        valueArr[(int)ConstValue.Enhances.AtkPow] = AtkPowValue;
        valueArr[(int)ConstValue.Enhances.AtkSpd] = AtkSpdValue;
        valueArr[(int)ConstValue.Enhances.CritChance] = CritChanceValue;
        valueArr[(int)ConstValue.Enhances.CritDmg] = CritDmgValue;
        valueArr[(int)ConstValue.Enhances.GoldUp] = GoldUpValue;
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