using UnityEngine;

public class ConstValue
{
    public const string EventSystem = "EventSystem";

    // notice
    public const string Notice_LessThan2 = "닉네임은 2자 이상이어야 합니다.";
    public const string Notice_NoBlankInName = "닉네임은 공백을 포함할 수 없습니다.";
    public const string Notice_NoSaveData = "저장된 데이터가 없습니다.";

    #region 열거형
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
