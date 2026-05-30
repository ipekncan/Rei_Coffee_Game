using UnityEngine;

public class CustomerController : MonoBehaviour
{

    public float speed = 3f; // Speed at which the customer moves
    public Transform target; // The target position the customer will move towards
    public float stoppingDistance = 0.05f; // Distance at which the customer will stop moving towards the target
    public float rotationSpeed = 5f; // Speed at which the customer rotates to face the target
    public bool isMoving = false;
    public bool isCompleted = false;
    public bool isActive = true;
    public int satisfactionLevel = 100; // Customer's satisfaction level (0-100) it will decrease if the customer waits too long or if the order is not fulfilled correctly

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (isActive && target != null)
        {
            isMoving = true;
            animator.SetBool("isStopped", false);
            MoveToTarget();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        MoveToTarget();
    }
    public void MoveToTarget()
    {
        if (isMoving && target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            // Rotate to face the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // Check if the customer has reached the target
            if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
            {
                isMoving = false;
                animator.SetBool("isStopped", true);
                isCompleted = true;
                OrderCoffee();
            }
        }
    }
    public void OrderCoffee()
    {
        // Simulate ordering coffee
        Debug.Log("Customer has ordered coffee.");
        // Here you can add logic to decrease satisfaction level if the order takes too long or is not fulfilled correctly
        OrderResult();
    }
    void OrderResult()
    {
        // Simulate the result of the order
        bool isOrderFulfilled = Random.value > 0.5f; // Randomly determine if the order is fulfilled correctly
        if (isOrderFulfilled)
        {
            Debug.Log("Customer received their coffee. Satisfaction level: " + satisfactionLevel);
        }
        else
        {
            satisfactionLevel -= 20; // Decrease satisfaction level if the order is not fulfilled correctly
            Debug.Log("Customer did not receive their coffee correctly. Satisfaction level: " + satisfactionLevel);
        }
        //after that we will deactivate the customer and reset the satisfaction level for the next time and delete the customer game object after a short delay
        DeactivateCustomer();
    }
    void DeactivateCustomer()
    {
        isActive = false;
        satisfactionLevel = 100; // Reset satisfaction level for the next time
        Destroy(gameObject, 2f); // Delete the customer game object after a short delay
    }
}
