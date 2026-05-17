using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class InventoryUIControl : MonoBehaviour
{
    public List<SlotUI> hotbarUiList = new List<SlotUI>();
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


        //Hotbar UI'sini güncellemek için benzer bir yöntem ekliyorum

        for (int i = 0; i<hotbarUiList.Count; i++)
        {
            var slotData=userInventory.playerInventory.inventorySlots[i];
            if (slotData.item != null)
            {
                Debug.Log("Hotbar'ýn " + i + ". slotuna þu item geliyor: " + slotData.item.itemName);
            }
            if (slotData.itemCount > 0 && slotData.item != null)
            {   
                hotbarUiList[i].itemImage.sprite = slotData.item.itemIcon;
                hotbarUiList[i].itemImage.color = Color.white;
                hotbarUiList[i].itemImage.enabled = true;
                if (slotData.item.isStackable)
                {
                    hotbarUiList[i].itemCountText.gameObject.SetActive(true);
                    hotbarUiList[i].itemCountText.text = slotData.itemCount.ToString();
                }
                else
                {
                    hotbarUiList[i].itemCountText.gameObject.SetActive(false);
                }
            }
            else
            {
                hotbarUiList[i].itemImage.sprite = null;
                hotbarUiList[i].itemImage.color = new Color(1, 1, 1, 0); 
                hotbarUiList[i].itemCountText.gameObject.SetActive(false);
            }
        }
    }

}
