// 무기 데이터 저장용
using System;
using System.Collections.Generic;

[Serializable]
public class WeaponData 
{
    public int WeaponIdx;
    public int AtkPower;
    public int CritChance;
    public int CritDamage;
    public int Cost;
}

[Serializable]
public class WeaponDataLoader: ILoader<int, WeaponData>
{
    public List<WeaponData> WeaponDatas = new List<WeaponData>();

    public Dictionary<int, WeaponData> MakeDic()
    {
        Dictionary<int, WeaponData> dic = new Dictionary<int, WeaponData>();
        foreach (WeaponData data in WeaponDatas)
            dic.Add(data.WeaponIdx, data);

        return dic;
    }
}