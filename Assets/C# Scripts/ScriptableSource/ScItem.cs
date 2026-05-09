using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/General Item")]
public class ScItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public bool isStackable;
    public Sprite itemIcon;//image of the item
    public GameObject itemModel;

}

public enum ItemType { Plant, Material, Tool, Weapon, Recipe, Product }
