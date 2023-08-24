using UnityEngine;

public class ConstValue
{
    public const string EventSystem = "EventSystem";

    // notice
    public const string Notice_LessThan2 = "�г����� 2�� �̻��̾�� �մϴ�.";
    public const string Notice_NoBlankInName = "�г����� ������ ������ �� �����ϴ�.";
    public const string Notice_NoSaveData = "����� �����Ͱ� �����ϴ�.";

    #region ������
    public enum GameDataVal
    {
        AtkPowLv,
        AtkSpdLv,
        CritChanceLv,
        CritDmgLv,
        GoldUpLv
    }
    #endregion
}
