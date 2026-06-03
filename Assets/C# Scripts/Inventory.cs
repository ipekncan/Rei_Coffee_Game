using UnityEngine;

public class Inventory:MonoBehaviour
{
    public ScInventory playerInventory;
    public InventoryUIControl uiInventory;
    bool isSwapping;
    int tempIndex;
    Slot tempSlot;

    private void Start()
    {
       
        uiInventory.UpdateUI();
        

    }

  
    public void SwappItem(int index)
    {
        if (playerInventory == null || playerInventory.inventorySlots == null)
        {
            Debug.LogError("Player Inventory (ScriptableObject) baðlý deðil!");
            return;
        }

        if (isSwapping == false)
        {
           
            tempIndex = index;
            tempSlot = playerInventory.inventorySlots[tempIndex];
            isSwapping = true;
            Debug.Log("Birinci slot seçildi: " + index + ". Þimdi yer deðiþtireceðiniz ikinci slota basýn.");
        }
        else
        {
            Debug.Log("Ýkinci slot seçildi: " + index + ". Yer deðiþtiriliyor...");

            playerInventory.inventorySlots[tempIndex] = playerInventory.inventorySlots[index];
            playerInventory.inventorySlots[index] = tempSlot;

            isSwapping = false;
            if (uiInventory != null)
            {
                uiInventory.UpdateUI();
                Debug.Log("Envanter ekraný güncellendi.");
            }
            else
            {
                Debug.LogWarning("uiInventory referansý eksik, ekran güncellenemedi!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemWorld groundItem = other.GetComponent<ItemWorld>();

            if (groundItem != null)
            {
             
                Collider col = other.GetComponent<Collider>();
                if (col != null) col.enabled = false;

                
                if (playerInventory != null)
                {
                    playerInventory.AddItem(groundItem.itemData, groundItem.amount);
                    uiInventory.UpdateUI();

                    
                    Destroy(other.gameObject);
                }
            }
        }
    }
}

