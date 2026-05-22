using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CafeMakerManager : MonoBehaviour
{
    public GameObject myUIPanel;
    [Header("Player Inventory")]
    public Inventory cafeMakerplayerInventory;

    [Header("UI Elements")]
    public Image[] inputSlots;      // Üstteki 3 adet malzeme görseli
    public Image resultSlot;        // Sonuç kahve görseli
    public Button brewButton;       // Demle butonu

    [Header("UI Elements - Panel Hotbar")]
    public Button[] hotbarButtons;  // Panel içindeki 7 adet seçim butonu
    public Button exitButton;        // Paneli kapatma butonu

    private List<ScItem> currentIngredients = new List<ScItem>();
    private ScRecipe currentMatchedRecipe;

    public void CloseMachineUI()
    {
        if(currentIngredients.Count > 0)
        {
            foreach(ScItem item in currentIngredients)
            {
               cafeMakerplayerInventory.playerInventory.AddItem(item, 1);
            }
            currentIngredients.Clear();
        }
        cafeMakerplayerInventory.uiInventory.UpdateUI();
        UpdateMakerUI();

            if(myUIPanel != null)
        {
            myUIPanel.SetActive(false);
        }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Makine UI kapatýldý, malzemeler envantere geri eklendi");
    }
    public void OpenMachineUI()
    {
        
            if (myUIPanel != null)
            {
                myUIPanel.SetActive(true);

                
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Debug.LogError("Makine üzerindeki scriptte 'myUIPanel' yok");
            } 
        }
    private void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(CloseMachineUI);
        }
        brewButton.interactable = false;
        ClearMaker();
        brewButton.onClick.AddListener(BrewCoffee);
    }

    private void OnEnable()
    {   //Cafe Maker Paneli açýldýðýnda envanteri güncelle
        RefreshHotbarUI();
    }

    public void RefreshHotbarUI()
    {
        if (cafeMakerplayerInventory == null || cafeMakerplayerInventory.playerInventory==null) return;

        var slots = cafeMakerplayerInventory.playerInventory.inventorySlots;

        for (int i = 0; i<hotbarButtons.Length; i++)
        {
            int index = i;

            if (i<slots.Count && slots[i].hasItem && slots[i].item != null)
            {
                ScItem item = slots[index].item;
                hotbarButtons[index].image.sprite = item.itemIcon;
                hotbarButtons[index].image.color = Color.white;

                hotbarButtons[index].onClick.RemoveAllListeners();
                hotbarButtons[index].onClick.AddListener(() => AddIngredientFromHotbar(item));
            }
            else
            {
                hotbarButtons[index].image.sprite = null;
                hotbarButtons[index].image.color = new Color(1, 1, 1, 0);
                hotbarButtons[index].onClick.RemoveAllListeners();
            }
        }
    }
        public void AddIngredientFromHotbar(ScItem item)
    {
        if (currentIngredients.Count<3)
        {  cafeMakerplayerInventory.playerInventory.RemoveItem(item, 1);
            currentIngredients.Add(item);
            UpdateMakerUI();
            CheckForMatchingRecipe();
        }
        else
        {
            Debug.Log("Zaten 3 malzeme var");
        }
    }

    public void UpdateMakerUI()
    {
        for (int i = 0; i<inputSlots.Length; i++)
        {
            if (i < currentIngredients.Count)
            {
                inputSlots[i].sprite = currentIngredients[i].itemIcon;
                inputSlots[i].color = Color.white;
            }
            else
            {
                inputSlots[i].sprite = null;
                inputSlots[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    private void CheckForMatchingRecipe()
    {
        currentMatchedRecipe = null;

        foreach(ScRecipe recipe in RecipeManager.learnedRecipes)
        {
            if (IsRecipeMatch(recipe))
            {
                currentMatchedRecipe = recipe;
                break;
            }
        }
        if (currentMatchedRecipe!=null)
        {
            resultSlot.sprite=currentMatchedRecipe.resultItem.itemIcon;
            resultSlot.color=Color.white;
            brewButton.interactable=true;
        }
        else { 
            resultSlot.sprite=null;
            resultSlot.color=new Color(1,1,1,0);
            brewButton.interactable=false;
        }
    }
    private bool IsRecipeMatch(ScRecipe recipe)
    {
        var currentGrouped=currentIngredients.GroupBy(x=>x.itemName).ToDictionary(g=>g.Key,g=>g.Count());
        var recipeGrouped = recipe.ingredients.ToDictionary(i => i.item.itemName, i => i.amount);

        if(currentGrouped.Count !=recipeGrouped.Count) return false;

        foreach(var requireditem in recipeGrouped)
        {
            if(!currentGrouped.ContainsKey(requireditem.Key) || currentGrouped[requireditem.Key] != requireditem.Value)
            {
                return false;
            }
            
        }
        return true;
    }
    public void BrewCoffee()
    { 
    if(currentMatchedRecipe !=null && cafeMakerplayerInventory !=null)
    {
            foreach(var ingredient in currentMatchedRecipe.ingredients)
            {
                cafeMakerplayerInventory.playerInventory.RemoveItem(ingredient.item, ingredient.amount);
            }
            cafeMakerplayerInventory.playerInventory.AddItem(currentMatchedRecipe.resultItem, 1);
            cafeMakerplayerInventory.uiInventory.UpdateUI();
            RefreshHotbarUI();

            Debug.Log($"Demlenen kahve: {currentMatchedRecipe.resultItem.itemName}");
            ClearMaker();


        }


    }

    public void ClearMaker()
    {
        foreach(ScItem scItem in currentIngredients)
        {
            // Malzemeleri geri envantere ekle
            cafeMakerplayerInventory.playerInventory.AddItem(scItem, 1);
        }
        currentIngredients.Clear();
        currentMatchedRecipe = null;
        UpdateMakerUI();
        resultSlot.sprite = null;
        resultSlot.color = new Color(1, 1, 1, 0);
        brewButton.interactable = false;
    }
}