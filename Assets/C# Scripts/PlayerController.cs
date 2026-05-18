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

   
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
       
        if (isDead || isInventoryOpen) return;

       
      
        if (value.isPressed && !isCarrying)
        {
            var slot = playerInventory.playerInventory.inventorySlots[selectedSlot];
            if (slot.item != null && slot.item.itemName == "Sword")
            {
                animator.SetTrigger("Attack");
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

            if (hitCollider.CompareTag("Machine") ||
                hitCollider.CompareTag("Item") ||
                hitCollider.CompareTag("FieldArea"))
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

            if (target != null)
            {
                
                if (target.CompareTag("Machine"))
                {
                    //machine interaction minigame trigger
                    animator.SetTrigger("isInteracting"); 
                }
                else if (target.CompareTag("Item"))
                {

                    animator.SetTrigger("UseItem"); 
                }

                else if (target.CompareTag("FieldArea"))
                {
                    animator.SetTrigger("isInteracting");
                    ToggleCarrying();
                }
            }
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
        ApplyGravity();
        MovePlayer();
        HandleRotation();
     
    }
    public void UpdateEquippedItem()
    {
        if (currentEquippedItem != null)
        {
            Destroy(currentEquippedItem);
        }

        var inventoryData = playerInventory.playerInventory.inventorySlots;
        if (selectedSlot < inventoryData.Count)
        {
            var slot = inventoryData[selectedSlot];

           
            if (slot.itemCount > 0 && slot.item != null && slot.item.itemPrefab != null)
            {
                currentEquippedItem = Instantiate(slot.item.itemPrefab, itemHolder);
                currentEquippedItem.transform.localPosition = Vector3.zero;
                currentEquippedItem.transform.localRotation = Quaternion.identity;
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