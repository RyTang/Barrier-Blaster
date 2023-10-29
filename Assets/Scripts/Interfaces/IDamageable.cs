/// <summary>
/// Any Object that is damageable, or that should be affected will need to implement this interface
/// </summary>
public interface IDamageable
{
    public bool TakeDamage(int damageAmount);
}
