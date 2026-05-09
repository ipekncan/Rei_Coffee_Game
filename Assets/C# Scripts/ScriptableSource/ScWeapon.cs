using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class ScWeapon : ScItem
{
    public int damage;
    public float attackSpeed;
}