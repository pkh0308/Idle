// ���� ��ǰ ������ �����
// MaxCount�� -1�̶�� ���� ��û ��ǰ
public class ShopData
{
    public readonly string Name;
    public readonly int MaxCount;
    public readonly int GoodsType;
    public readonly int GoodsValue;

    public ShopData(string Name, int MaxCount, int GoodsType, int GoodsValue)
    {
        this.Name = Name;
        this.MaxCount = MaxCount;
        this.GoodsType = GoodsType;
        this.GoodsValue = GoodsValue;
    }
}