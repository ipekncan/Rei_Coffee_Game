using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f; 
    public float rotationSpeed = 700f;

    private Animator animator;
    private CharacterController controller;
    private Vector3 movement;
    private float verticalVelocity;
    private bool isSprinting = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        movement = new Vector3(inputVector.x, 0, inputVector.y);
    }

   
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            
            GameObject target = GetNearbyObject();

            if (target != null)
            {
                
                if (target.CompareTag("Machine"))
                {
                    animator.SetTrigger("Interact"); 
                }
                else if (target.CompareTag("Item"))
                {
                    animator.SetTrigger("UseItem"); 
                }
            }
        }
    }
    void Update()
    {
        
        if (controller.isGrounded) verticalVelocity = -0.5f;
        else verticalVelocity += Physics.gravity.y * Time.deltaTime;

    
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        Vector3 velocity = movement * currentSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        
        float animSpeed = movement.magnitude * (isSprinting ? 2.0f : 1.0f);
        animator.SetFloat("Speed", animSpeed);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // This method checks for nearby objects within a certain radius and returns the first one that is tagged as "Machine" or "Item".So we can use 'E' for many purposes
    private GameObject GetNearbyObject()
    {
        float detectionRadius = 2.0f; 
                                      
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider.CompareTag("Machine") || hitCollider.CompareTag("Item"))
            {
                return hitCollider.gameObject;
            }
        }
        return null; 
    }
}