using System.Security.Cryptography;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public ScPlant SaplingData;
    private int currentStage = 1;
    private float timer = 0;
    private MeshFilter meshFilter;
    public bool isHarvestable = false;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        UpdateVisual();
    }

    void Update()
    {
        if (currentStage < 3)
        {
            timer += Time.deltaTime;
            if (timer >= SaplingData.growthTime)
            {
                Grow();
            }
        }
    }

    void Grow()
    {
        currentStage++;
        timer = 0;
        UpdateVisual();

        if (currentStage == 3) isHarvestable = true;
    }

    void UpdateVisual()
    {
        if (currentStage == 1) meshFilter.mesh = SaplingData.stage1Mesh;
        else if (currentStage == 2) meshFilter.mesh = SaplingData.stage2Mesh;
        else if (currentStage == 3) meshFilter.mesh = SaplingData.stage3Mesh;
    }

    public void Harvest()
    {
        if (isHarvestable)
        {




            Inventory playerInv = Object.FindObjectOfType<Inventory>();

            if (playerInv != null || playerInv.playerInventory !=null)
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


