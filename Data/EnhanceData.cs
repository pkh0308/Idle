using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceData
{
    public readonly int AtkPowCost;
    public readonly int AtkSpdCost;
    public readonly int CritChanceCost;
    public readonly int CritDmgCost;
    public readonly int GoldUpCost;
    int[] costArr = new int[ConstValue.NumOfEnhance];

    public readonly int AtkPowValue;
    public readonly int AtkSpdValue;
    public readonly int CritChanceValue;
    public readonly int CritDmgValue;
    public readonly int GoldUpValue;
    int[] valueArr = new int[ConstValue.NumOfEnhance];

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
        costArr[0] = AtkPowCost;
        costArr[1] = AtkSpdCost;
        costArr[2] = CritChanceCost;
        costArr[3] = CritDmgCost;
        costArr[4] = GoldUpCost;

        valueArr[0] = AtkPowValue;
        valueArr[1] = AtkSpdValue;
        valueArr[2] = CritChanceValue;
        valueArr[3] = CritDmgValue;
        valueArr[4] = GoldUpValue;
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