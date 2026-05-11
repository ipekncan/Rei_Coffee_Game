using UnityEngine;

public class Inventory:MonoBehaviour
{
    public ScInventory playerInventory;
    public InventoryUIControl uiInventory;
    bool isSwapping;

    private void Start()
    {
      uiInventory.UpdateUI();
    }
    public void SwappItem(int fromIndex, int toIndex)
    {
       if(fromIndex<0 || fromIndex >= playerInventory.inventorySlots.Count || toIndex < 0 || toIndex >= playerInventory.inventorySlots.Count)
       {
           Debug.LogError("DÝKKAT: Geçersiz indeks! Lütfen geçerli bir indeks aralýðý girin.");
           return;
        }

       Slot bufferSlot=playerInventory.inventorySlots[fromIndex];
        playerInventory.inventorySlots[fromIndex]=playerInventory.inventorySlots[toIndex];
        playerInventory.inventorySlots[toIndex]=bufferSlot;
        uiInventory.UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemWorld groundItem = other.GetComponent<ItemWorld>();

            if (groundItem != null)
            {
                if (playerInventory != null) 
                {
                    playerInventory.AddItem(groundItem.itemData, groundItem.amount);
                    Debug.Log(groundItem.itemData.itemName + " baþarýyla listeye gönderildi!");
                    uiInventory.UpdateUI();
                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.LogError("DÝKKAT: Player üzerindeki Inventory scriptinde ScInventory dosyasý eksik!");
                }
            }
        }
    }
}

