using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Inventory UI")]
    public GameObject inventoryPanel; 
    private bool isInventoryOpen = false;
    public int selectedSlot = 0; //hotbar için seçili slot indexi

    [Header("Player Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f; 
    public float rotationSpeed = 700f;
    
    [Header("Player States")]
    public int playerHealth = 100;
    public int playerMoney = 0;
    public bool isDead = false;

    //state variables
    private bool isSprinting = false;
    private bool isCarrying = false;

    //carrying selected item
    [Header("Inventory Reference")]
    public Inventory playerInventory;
    public Transform itemHolder;
    private GameObject currentEquippedItem;

    private Animator animator;
    private CharacterController controller;
    private Vector3 movement;
    private float verticalVelocity;

    [Header("Planting Settings")]
    public GameObject plantPrefab;

    EnemyController enemy; 

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("InventoryImage");
        }
    }

    //-----INPUT SYSTEM ACTIONS -----//
    public void OnMove(InputValue value)
    {
        if (isDead == true ) { return; }
        Vector2 inputVector = value.Get<Vector2>();
        movement = new Vector3(inputVector.x, 0, inputVector.y);
    }

   
    //public void OnSprint(InputValue value)
    //{
    //    isSprinting = value.Get<float>() > 0.5f; 
    //}

    public void OnAttack(InputValue value)
    {
        if (isDead || isInventoryOpen) return;

        if (value.isPressed && !isCarrying)
        {
            var slot = playerInventory.playerInventory.inventorySlots[selectedSlot];
            if (slot.item != null && slot.item.itemName == "Sword")
            {
                animator.SetTrigger("Attack");

                // En yakýn enemy'yi bul ve hasar ver
                Collider[] hits = Physics.OverlapSphere(transform.position, 2.5f);
                foreach (var hit in hits)
                {
                    EnemyController ec = hit.GetComponent<EnemyController>();
                    if (ec != null)
                    {
                        ec.TakeDamage(20);
                        break; // ilk bulunan enemy'ye vur
                    }
                }
            }
            else
            {
                Debug.Log("Saldýrmak için kýlýç kuþanmalýsýn!");
            }
        }
    }

    // This method checks for nearby objects within a certain radius and returns the first one that is tagged as "Machine" or "Item".So we can use 'E' for many purposes
    private GameObject GetNearbyObject()
    {
        float detectionRadius = 2.0f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<PlantBehaviour>() != null) return hitCollider.gameObject;

            if (hitCollider.CompareTag("Machine") ||
                hitCollider.CompareTag("Item") ||
                hitCollider.CompareTag("FieldArea") ||
                hitCollider.CompareTag("Recipe"))
            {
                return hitCollider.gameObject;
            }
        }
        return null;
    }
    public void OnInteract(InputValue value)
    {
        if (value.isPressed && !isDead)
        {
            GameObject target = GetNearbyObject();
            Debug.Log("Etkileþimde bulunulacak nesne: " + (target != null ? target.name : "Yok"));

            if (target != null)
            {
               
                if (target.TryGetComponent<PlantBehaviour>(out PlantBehaviour plant))
                {
                    if (plant.isHarvestable)
                    {
                        animator.SetTrigger("isInteracting");
                        plant.Harvest();
                        Debug.Log("Bitki hasat edildi: " + plant.SaplingData.itemName);
                    }
                    else
                    {
                        Debug.Log("Bu bitki henüz büyüme aþamasýnda, hasat edilemez!");
                    }

                    
                    return;
                }


                if (target.CompareTag("Machine"))
                {

                    CafeMakerManager manager = target.GetComponent<CafeMakerManager>();
                    if (manager != null)
                    {
                        animator.SetTrigger("isInteracting");
                        manager.OpenMachineUI();
                        Debug.Log("Makine ile etkileþim gerçekleþti: " + target.name);
                    }
                    else
                    {
                        Debug.LogError("HATA: 'Machine' tagine sahip nesnede CafeMakerManager scripti bulunamadý!");
                    }
                }
                else if (target.CompareTag("Recipe"))
                {
                    if (target.TryGetComponent<ItemWorld>(out ItemWorld itemWorld))
                    //TryGetComponent ile ItemWorld scripti var mý kontrol ediyoruz, varsa itemWorld deðiþkenine atýyoruz
                    {

                        if (itemWorld.itemData is ScRecipe recipe)
                        {
                            animator.SetTrigger("isInteracting");
                            RecipeManager.Instance.LearnNewRecipe(recipe);
                            Destroy(target);
                            return;
                        }
                        animator.SetTrigger("UseItem");
                    }

                }
                else if (target.CompareTag("FieldArea"))
                {
                    Debug.Log("Tarla alanýna etkileþim gerçekleþti.");
                    var currentSlot = playerInventory.playerInventory.inventorySlots[selectedSlot];

                    if (currentSlot.itemCount > 0 && currentSlot.item is ScPlant saplingData)
                    {
                        Debug.Log("Fidan ekme iþlemi gerçekleþiyor: " + saplingData.itemName);
                        animator.SetTrigger("isInteracting");
                        PlantSapling(target.transform.position, saplingData);

                        playerInventory.playerInventory.RemoveItem(currentSlot.item, 1);
                        playerInventory.uiInventory.UpdateUI();
                    }
                    else
                    {
                        Debug.Log("Ekmek için geçerli bir fidan seçili deðil.");
                        animator.SetTrigger("isInteracting");
                    }
                }
            }
        }
    }
    private void PlantSapling(Vector3 position, ScPlant saplingData)
    {
        GameObject newPlant = Instantiate(plantPrefab, position, Quaternion.identity);

        newPlant.tag = "Plant"; 

        if (newPlant.TryGetComponent<PlantBehaviour>(out PlantBehaviour plantbh))
        {
            plantbh.SaplingData = saplingData;
            Debug.Log("Bitki baþarýyla ekildi ve script baðlandý.");
        }
        else
        {
            Debug.LogError("HATA: Ektiðin 'plantPrefab' üzerinde PlantBehaviour scripti bulunamadý!");
        }
    }
    private void ToggleCarrying()
    {
        isCarrying = !isCarrying;
        animator.SetBool("IsCarrying", isCarrying);
        Debug.Log("Carrying state toggled: " + isCarrying);

    }

    public void OnToggleInventory(InputValue value)
    {
        if (value.isPressed && !isDead)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
        else
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    private void OnHotbarInteraction(InputValue value)
    {
        
        float keyFieldValue = value.Get<float>();
        if (keyFieldValue <= 0) return;

        int index = (int)keyFieldValue - 1;
        if (index >= 0 && index < 7)
        {
            selectedSlot = index;
            Debug.Log("Seçilen eþya slotu: " + selectedSlot);
            UpdateEquippedItem();
        }
    }
    void Update()
    {
        if(isDead== true || isInventoryOpen==true) { return; }

        isSprinting = Keyboard.current.leftShiftKey.isPressed;

        ApplyGravity();
        MovePlayer();
        HandleRotation();
     
    }
    public void UpdateEquippedItem()
    {
        Debug.Log($"UpdateEquippedItem çaðrýldý! Çaðýran obje: {gameObject.name}", gameObject);
        if (currentEquippedItem != null)
        {
            Destroy(currentEquippedItem);
            currentEquippedItem = null; 
        }

        var inventoryData = playerInventory.playerInventory.inventorySlots;
        if (selectedSlot < inventoryData.Count)
        {
            var slot = inventoryData[selectedSlot];

           
            if (slot.itemCount > 0 && slot.item != null && slot.item.itemPrefab != null)
            {
                if (slot.item is ScPlant plantItem && plantItem.equippedHandPrefab != null)
                {
                    currentEquippedItem = Instantiate(plantItem.equippedHandPrefab, itemHolder);
                }
                else if (slot.item.itemPrefab != null)
                {
                    currentEquippedItem = Instantiate(slot.item.itemPrefab, itemHolder);
                }

                if (currentEquippedItem != null)
                {
                    currentEquippedItem.transform.localPosition = Vector3.zero;
                    currentEquippedItem.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    void MovePlayer()
    {
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;
        Vector3 velocity = movement * currentSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        // Blend Tree Parameter (0: Idle, 1: Walk, 2: Run)
        float animSpeed = movement.magnitude * (isSprinting ? 2.0f : 1.0f);
        animator.SetFloat("Speed", animSpeed);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded) verticalVelocity = -0.5f;
        else verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

   

    public void TakeDamage(int damage)
    {
        if (isDead==true) { return; }
        playerHealth -= damage;
        animator.SetTrigger("TakeDamage");
        if (playerHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        movement = Vector3.zero;
        
    }

    //we can see the detection radius in the editor for debugging purposes, this will help us to adjust the radius for better gameplay experience
    private void OnDrawGizmos()
    {
   
        Gizmos.color = new Color(0, 1, 0, 0.3f);//green
        Vector3 detectionCenter = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawSphere(detectionCenter, 2.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionCenter, 2.0f);
    }
}