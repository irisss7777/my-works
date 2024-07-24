
public interface IDamageble
{
    public bool TryAttackThis(int[] damageTypeNCount, IDamageble damageble);

    public void GetBonus(out int bonusType, out int bonusValue);
}
