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
            var slotData = userInventory.playerInventory.inventorySlots[i];

            if (slotData.itemCount > 0 && slotData.item != null)
            {
                slotUiList[i].itemImage.sprite = slotData.item.itemIcon;
                slotUiList[i].itemImage.color = Color.white;
                slotUiList[i].itemImage.enabled = true;

                if (slotData.item.isStackable)
                {
                    slotUiList[i].itemCountText.gameObject.SetActive(true);
                    slotUiList[i].itemCountText.text = slotData.itemCount.ToString();
                }
                else
                {
                    slotUiList[i].itemCountText.gameObject.SetActive(false);
                }
            }
            else
            {
                slotUiList[i].itemImage.sprite = null;
                slotUiList[i].itemImage.color = new Color(1, 1, 1, 0); 

                slotUiList[i].itemCountText.gameObject.SetActive(false);
            }
        }
    }
}
