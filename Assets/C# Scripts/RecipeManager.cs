using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    public static List<ScRecipe> learnedRecipes= new List<ScRecipe>();

    [Header("UI References")]

    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public GameObject recipePanel;
    public TextMeshProUGUI recipeText;

    void Awake()
    {
      if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
      else      {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateRecipeUI();
    }

    public void LearnNewRecipe(ScRecipe newRecipe)
    {
        if (learnedRecipes.Contains(newRecipe))
        {
            Debug.Log("Bu tarif zaten öðrenildi: " + newRecipe.itemName);
            
            return;
        }
        if (!learnedRecipes.Contains(newRecipe))
        {
            learnedRecipes.Add(newRecipe);
            if (notificationPanel != null && recipeText != null) { 
                ShowNewRecipeNotification();
                
            }
            UpdateRecipeUI();
        }
    }

    void ShowNewRecipeNotification()
    {
        notificationPanel.SetActive(true);
        CancelInvoke("HideNotification");
        Invoke("HideNotification", 3f);
        notificationText.text = "New Recipe Learned: " + learnedRecipes[learnedRecipes.Count - 1].itemName;
        //üst üste tarif bildirimi gelmesi durumunda sýfýrlamak için
    }
    void HideNotification()
    {
        notificationPanel.SetActive(false);
    }

    public void UpdateRecipeUI()
    {
        if (recipeText==null) return;
        recipeText.text = "Learned Recipes:\n";
        foreach(var recipe in learnedRecipes)
        {
            recipeText.text += recipe.GetRecipeDescription() + "\n";
        }
    }
}
