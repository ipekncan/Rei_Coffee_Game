using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ScItem itemData;
    public int amount = 1;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Start()
    {
        if (itemData != null)
        {
            SetupItem();

        }
    }

    public void SetupItem()
    {
        
        if (GetComponentInChildren<MeshFilter>() != null)
        {
            return;
        }

        if (itemData != null && itemData.itemModel != null)
        {

            foreach (Transform child in transform) { Destroy(child.gameObject); }

            GameObject spawnedModel = Instantiate(itemData.itemModel, transform);

            if (spawnedModel.TryGetComponent<ItemWorld>(out ItemWorld component))
            {
                Destroy(component);
            }

            if (spriteRenderer != null) spriteRenderer.enabled = false;
        }
    }

}


