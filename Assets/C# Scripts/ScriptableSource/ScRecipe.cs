using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class ScRecipe : ScItem
{
    [Header("Tarif Icerigi")]
    public List<Ingredient> ingredients; 
    public ScItem resultItem;
    public string GetRecipeDescription()
    {
        string description = "Recipe:\n";
        foreach (var ingredient in ingredients)
        {
            description += $"{ingredient.amount} x {ingredient.item.itemName}\n";
        }
        description += $"Result: {resultItem.itemName}";
        return description;
    }
    

}

[System.Serializable]
public class Ingredient
{
    public ScItem item; 
    public int amount;  
}
