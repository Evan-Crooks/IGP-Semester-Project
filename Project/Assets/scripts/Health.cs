using UnityEngine;


class Health : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int armor;

    public int DealDamage(int damage, int armorPen)
    {
        // subject to change: total damage = baseDamage - max((armor + ArmorPen), 0)
        health -= damage - ((armor - armorPen) < 0 ? 0 : armor - armorPen);
        return health;
    }
}