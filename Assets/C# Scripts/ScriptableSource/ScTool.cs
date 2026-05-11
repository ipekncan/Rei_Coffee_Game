using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Tool")]
public class ScTool : ScItem
{
    public float toolRange;
    public int maxCapacity; // Suluk için 5 kullaným gibi
}