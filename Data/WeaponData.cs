// ���� ������ �����
public class WeaponData 
{
    public readonly int AtkPower;
    public readonly int CritChance;
    public readonly int CritDamage;
    public readonly int Cost;

    public WeaponData(int AtkPower, int CritChance, int CritDamage, int Cost)
    {
        this.AtkPower = AtkPower;
        this.CritChance = CritChance;
        this.CritDamage = CritDamage;
        this.Cost = Cost;
    }
}