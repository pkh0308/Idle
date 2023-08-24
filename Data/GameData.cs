// 게임 데이터 저장용 클래스
public class GameData
{
    public readonly int AtkPowLv;
    public readonly int AtkSpdLv;
    public readonly int CritChanceLv;
    public readonly int CritDmgLv;
    public readonly int GoldUpLv;

    public GameData(int atkPowLv, int atkSpdLv, int critChanceLv, int critDmgLv, int goldUpLv)
    {
        AtkPowLv = atkPowLv;
        AtkSpdLv = atkSpdLv;
        CritChanceLv = critChanceLv;
        CritDmgLv = critDmgLv;
        GoldUpLv = goldUpLv;
    }

    public int GetValue(ConstValue.GameDataVal value)
    {
        switch (value)
        {
            case ConstValue.GameDataVal.AtkPowLv:
                return AtkPowLv;
            case ConstValue.GameDataVal.AtkSpdLv:
                return AtkSpdLv;
            case ConstValue.GameDataVal.CritChanceLv:
                return CritChanceLv;
            case ConstValue.GameDataVal.CritDmgLv:
                return CritDmgLv;
            case ConstValue.GameDataVal.GoldUpLv:
                return GoldUpLv;
            default: 
                return -1;
        }
    }
    public string GetName(int idx)
    {
        switch (idx)
        {
            case (int)ConstValue.GameDataVal.AtkPowLv:
                return AtkPowLv.ToString();
            case (int)ConstValue.GameDataVal.AtkSpdLv:
                return AtkSpdLv.ToString();
            case (int)ConstValue.GameDataVal.CritChanceLv:
                return CritChanceLv.ToString();
            case (int)ConstValue.GameDataVal.CritDmgLv:
                return CritDmgLv.ToString();
            case (int)ConstValue.GameDataVal.GoldUpLv:
                return GoldUpLv.ToString();
            default:
                return null;
        }
    }
}