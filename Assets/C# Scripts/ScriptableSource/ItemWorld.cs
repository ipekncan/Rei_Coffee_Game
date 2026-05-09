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
        if (itemData.itemModel != null)
        {
            Instantiate(itemData.itemModel, transform);
        }
        else if (itemData.itemIcon != null)
        {

            spriteRenderer.sprite = itemData.itemIcon;
        }
    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(itemData.itemName + " toplandý!");
            Destroy(gameObject);
        }
    }
}


