// 보스 데이터 저장용
public class BossData
{
    public readonly int Hp;
    public readonly int BossId;
    public readonly string BossName;
    public readonly int TimeLimit;
    public readonly int DropGold;
    public readonly int DropGem;

    public BossData(int hp, int id, string name, int timeLimit, int gold, int gem)
    {
        Hp = hp;
        BossId = id;
        BossName = name;
        TimeLimit = timeLimit;
        DropGold = gold;
        DropGem = gem;
    }
}