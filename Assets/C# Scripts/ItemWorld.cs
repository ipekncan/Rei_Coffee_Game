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
        foreach (Transform child in transform) { Destroy(child.gameObject); }
        if (itemData.itemModel != null)
        {
            Instantiate(itemData.itemModel, transform);
            if (spriteRenderer != null) spriteRenderer.enabled = false;
        }
        else if (itemData.itemIcon != null && spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = itemData.itemIcon;
        }
    }
     
}


