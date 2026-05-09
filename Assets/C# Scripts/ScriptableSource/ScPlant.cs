using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Inventory/Plant")]
public class ScPlant : ScItem
{
    public float growthTime; 
    public ScItem harvestResult; // Büyüyünce hangi çekirdeði verecek?
}
