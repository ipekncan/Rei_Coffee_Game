using NUnit.Framework.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Inventory/Plant")]
public class ScPlant : ScItem
{
    public int yieldAmount = 3;
    public float levelgrowthTime = 10f; // Her aþama arasý süre
    [Header("Visuals")]
    public Mesh stage1Mesh; 
    public Mesh stage2Mesh; 
    public Mesh stage3Mesh;

    [Header("Harvest")]
   
    public float growthTime; 
    public ScItem harvestResult; 


}
