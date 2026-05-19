using NUnit.Framework.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Inventory/Plant")]
public class ScPlant : ScItem
{
    public int yieldAmount = 3;
    public float levelgrowthTime = 10f; // Her aþama arasý süre
   

    [Header("Visuals (Equiped Version)")]
    // Eþya hotbarda seçildiðinde oyuncunun elinde sadece bu sade prefab olacak
    public GameObject equippedHandPrefab;

    [Header("Harvest")]
    public ScItem harvestResult; 


}
