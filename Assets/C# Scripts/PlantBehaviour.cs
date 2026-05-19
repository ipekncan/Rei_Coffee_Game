using System.Security.Cryptography;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public ScPlant SaplingData;
    public int currentStage = 1;
    private float timer = 0;

    public GameObject stage1Obj;
    public GameObject stage2Obj;
    public GameObject stage3Obj;

    public bool isHarvestable = false;

    void Start()
    {
        if (SaplingData == null)
        {
            Debug.LogError((gameObject.name)+"üzerinde SaplingData (ScriptableObject) eksik!");
            return;
        }
        UpdateVisual();
    }

    void Update()
    {
        if (currentStage < 3)
        {
            timer += Time.deltaTime;
            if (timer >= SaplingData.levelgrowthTime)
            {   Debug.Log("Büyüme zamaný geldi! Aþama: " + currentStage);
                Grow();
            }
        }
    }

    void Grow()
    {
        currentStage++;
        timer = 0;
        UpdateVisual();
        Debug.Log("Bitki büyüdü! Þu anki aþama: " + currentStage);
        if (currentStage == 3) isHarvestable = true;
    }



    void UpdateVisual()
    {
        // Önce hepsini kapat
        if (stage1Obj) stage1Obj.SetActive(false);
        if (stage2Obj) stage2Obj.SetActive(false);
        if (stage3Obj) stage3Obj.SetActive(false);

        // Sadece mevcut aþamayý aç
        switch (currentStage)
        {
            case 1: if (stage1Obj) stage1Obj.SetActive(true); break;
            case 2: if (stage2Obj) stage2Obj.SetActive(true); break;
            case 3: if (stage3Obj) stage3Obj.SetActive(true); break;
        }

        Debug.Log($"Görsel Degisti: Aþama {currentStage} aktif.");
    }

    public void Harvest()
    {
        if (isHarvestable)
        {




            Inventory playerInv = Object.FindFirstObjectByType<Inventory>();

            if (playerInv != null && playerInv.playerInventory !=null)
            {


                playerInv.playerInventory.AddItem(SaplingData.harvestResult, SaplingData.yieldAmount);
                playerInv.uiInventory.UpdateUI();
                Debug.Log(" toplandý!");
                Destroy(gameObject);


            }
            else { Debug.Log("Oyuncu envanteri bulunamadý"); }
        }
        else { Debug.Log("Henüz Hasata Hazir Degil"); }
    }
}


