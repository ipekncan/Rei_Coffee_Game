using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class ScRecipe : ScItem
{
    [Header("Tarif Ýçeriði")]
    public List<Ingredient> ingredients; 
    public ScItem resultItem;          

         
}

[System.Serializable]
public class Ingredient
{
    public ScItem item; 
    public int amount;  
}
