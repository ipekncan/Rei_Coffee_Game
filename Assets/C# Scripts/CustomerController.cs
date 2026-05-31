using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public Image fillBar;
    public float waitTime = 30f;
    public float currentWaitTime = 0f;
    public bool isOrderTaken = false;

    public float speed = 3f;
    public Transform target;
    public float stoppingDistance = 0.05f;
    public float rotationSpeed = 5f;
    public float satisfactionLevel = 100f;

    public bool isMoving = false;
    public bool isWaiting = false;   // hedefe ulaþtý, bekliyor
    public bool isFinished = false;  // sipariþ tamamlandý

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (target != null)
        {
            isMoving = true;
            animator.SetBool("isStopped", false);
        }
    }

    void Update()
    {
        if (isFinished) return;
        if (isMoving)
            MoveToTarget();
        if (isWaiting)
            UpdateBar();
    }

    void MoveToTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
        {
            isMoving = false;
            isWaiting = true;
            animator.SetBool("isStopped", true);


        }
    }

    void UpdateBar()
    {
        if (fillBar == null) return;

        currentWaitTime += Time.deltaTime;

        float fillAmount = waitTime > 0
            ? 1f - (currentWaitTime / waitTime)
            : 0f;
        fillAmount = Mathf.Clamp01(fillAmount);

        fillBar.fillAmount = fillAmount;
        fillBar.color = Color.Lerp(Color.red, Color.green, fillAmount);

        // Süre doldu, sabýrsýzca ayrýl
        if (currentWaitTime >= waitTime)
        {
            isWaiting = false;
            isFinished = true;
            fillBar.gameObject.SetActive(false);
            float finalSatisfaction = isOrderTaken ? satisfactionLevel : 0f;
            OrderResult(finalSatisfaction);
            DeactivateCustomer();
        }
    }

    public void OrderCoffee()
    {
        satisfactionLevel = 100f - (currentWaitTime / waitTime) * 100f;
        OrderResult(satisfactionLevel);
    }

    void OrderResult(float satisfaction)
    {
        Debug.Log($"Sipariþ {(isFinished ? "tamamlandý" : "baþarýsýz")}. Memnuniyet: {satisfaction}");
    }
    public void TakeOrder()
    {
        if (!isWaiting || isFinished || isOrderTaken) return;
        isOrderTaken = true;
        OrderCoffee();
    }

    void DeactivateCustomer()
    {
        satisfactionLevel = 100f;
        Destroy(gameObject, 2f);
    }
}