// ���� ��ǰ ������ �����
// MaxCount�� -1�̶�� ���� ��û ��ǰ
using System;
using System.Collections.Generic;

[Serializable]
public class ShopData
{
    public int Id;
    public string Name;
    public int Cost;
    public int MaxCount;
    public int GoodsType;
    public int GoodsValue;
}

[Serializable]
public class ShopDataLoader : ILoader<int, ShopData>
{
    public List<ShopData> ShopDatas = new List<ShopData>();

    public Dictionary<int, ShopData> MakeDic()
    {
        Dictionary<int, ShopData> dic = new Dictionary<int, ShopData>();
        foreach (ShopData data in ShopDatas)
            dic.Add(data.Id, data);

        return dic;
    }
}