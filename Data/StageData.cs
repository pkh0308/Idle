// 스테이지 데이터 저장용
public class StageData
{
    public readonly int Hp;
    public readonly int EnemyCount;
    public readonly int MinEnemyId;
    public readonly int MaxEnemyId;
    public readonly int DropGold;
    public readonly int DropGem;

    public StageData(int hp, int count, int minId, int maxId, int gold, int gem)
    {
        Hp = hp;
        EnemyCount = count;
        MinEnemyId = minId;
        MaxEnemyId = maxId;
        DropGold = gold;
        DropGem = gem;
    }
}