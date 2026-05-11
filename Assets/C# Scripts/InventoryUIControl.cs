using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class InventoryUIControl : MonoBehaviour
{
    public List<SlotUI> slotUiList = new List<SlotUI>();
    private Inventory userInventory;
    private void Awake()
    {
        userInventory = GetComponent<Inventory>();
    }
    public void UpdateUI()
    {
        

        for (int i = 0; i < slotUiList.Count; i++)
        {
            
            if (userInventory.playerInventory.inventorySlots[i].itemCount > 0)
            {
                
                slotUiList[i].itemImage.sprite = userInventory.playerInventory.inventorySlots[i].item.itemIcon;
                slotUiList[i].itemImage.enabled = true; 

                
                if (userInventory.playerInventory.inventorySlots[i].item.isStackable == true)
                {
                    slotUiList[i].itemCountText.gameObject.SetActive(true);
                    slotUiList[i].itemCountText.text = userInventory.playerInventory.inventorySlots[i].itemCount.ToString();
                }
                else
                {
                   
                    slotUiList[i].itemCountText.gameObject.SetActive(false);
                }
            }
            
            else
            {
                slotUiList[i].itemImage.sprite = null;
                slotUiList[i].itemImage.enabled = false; 
                slotUiList[i].itemCountText.gameObject.SetActive(false);
                slotUiList[i].itemCountText.text = "";
            }
        }
    }
}
