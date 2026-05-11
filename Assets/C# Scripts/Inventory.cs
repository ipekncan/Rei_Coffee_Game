using UnityEngine;

public class Inventory:MonoBehaviour
{
    public ScInventory playerInventory;

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

