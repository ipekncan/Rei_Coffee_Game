using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;



[System.Serializable]
public class Slot
{
    public bool hasItem;
    public int itemCount;
    public ScItem item;

}
[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/NewInventory")]
public class ScInventory : ScriptableObject
{
   public List<Slot> inventorySlots = new List<Slot>();
    int stackLimit = 64;
    public void AddItem(ScItem item, int count)
    {
        if (item == null || count <= 0) return;

        // Mevcut slotlarda "Yer Var mý?" kontrolü (Stacking)
        if (item.isStackable)
        {
            foreach (Slot slot in inventorySlots)
            {
                // Slot dolu mu? Ayný item mý? Ve limitin altýnda mý?
                if (slot.hasItem && slot.item == item && slot.itemCount < stackLimit)
                {
                    int availableSpace = stackLimit - slot.itemCount;
                    int itemsToAdd = Mathf.Min(availableSpace, count);

                    slot.itemCount += itemsToAdd;
                    count -= itemsToAdd;

                    // Eðer tüm eþyalar yerleþtiyse fonksiyondan çýk
                    if (count <= 0) return;
                }
            }
        }

        //  Kalan eþyalar için BOÞ SLOTLARI doldur
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].hasItem)
            {
                inventorySlots[i].item = item;
                inventorySlots[i].hasItem = true;

                //  Tek seferde en fazla stackLimit kadar ekle
                int itemsToAdd = Mathf.Min(stackLimit, count);
                inventorySlots[i].itemCount = itemsToAdd;
                count -= itemsToAdd;

                // Eðer hala eþya varsa döngü devam edip BÝR SONRAKÝ boþ slotu arayacak
                if (count <= 0) return;
            }
        }

        // Boþ slot bittiyse ama hala eþya varsa (Envanter dolu demektir)
        if (count > 0)
        {
            Debug.LogWarning("Envanterde yer kalmadý! Kalan miktar: " + count);
        }
    }
}

//Serializable: bir programlama nesnesinin durumunu (içindeki verileri) saklanabilir veya að üzerinden gönderilebilir bir formata (byte akýþý, JSON, XML) dönüþtürülebilir hale getiren bir arayüz veya iþaretleme yöntemidir


