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
    public bool isWaiting = false;
    public bool isFinished = false;

    [Header("Recipe UI")]
    public GameObject recipeIconObject;
    private Image recipeIconImage;

    private ScRecipe requestedRecipe;

    private Animator animator;

    void OnEnable()
    {
        RecipeManager.OnRecipeLearned += CheckForNewRecipe;
    }

    void OnDisable()
    {
        RecipeManager.OnRecipeLearned -= CheckForNewRecipe;
    }

    void CheckForNewRecipe()
    {
        if (requestedRecipe == null)
            AssignRandomRecipe();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (recipeIconObject != null)
            recipeIconImage = recipeIconObject.GetComponent<Image>();
        if (target != null)
        {
            isMoving = true;
            animator.SetBool("isStopped", false);
        }
        AssignRandomRecipe();
    }

    void Update()
    {
        if (isFinished) return;
        if (isMoving)
            MoveToTarget();
        if (isWaiting)
            UpdateBar();
    }

    void LateUpdate()
    {
        if (recipeIconObject != null && recipeIconObject.activeSelf)
            recipeIconObject.transform.parent.LookAt(Camera.main.transform);
    }

    void AssignRandomRecipe()
    {
        if (RecipeManager.learnedRecipes == null || RecipeManager.learnedRecipes.Count == 0)
        {
            Debug.Log("Henüz hiç tarif öðrenilmemiþ.");
            if (recipeIconObject != null) recipeIconObject.SetActive(false);
            return;
        }

        int randomIndex = Random.Range(0, RecipeManager.learnedRecipes.Count);
        requestedRecipe = RecipeManager.learnedRecipes[randomIndex];

        Sprite icon = requestedRecipe.itemIcon;

        if (icon == null)
            icon = Resources.Load<Sprite>($"Icons/{requestedRecipe.resultItem.itemName}");

        if (recipeIconImage != null && icon != null)
        {
            recipeIconImage.sprite = icon;
            recipeIconObject.SetActive(true);
            Debug.Log("Icon atandý: " + requestedRecipe.resultItem.itemName);
        }
        else
        {
            Debug.LogWarning($"Icon bulunamadý: {requestedRecipe?.resultItem?.itemName}");
            if (recipeIconObject != null) recipeIconObject.SetActive(false);
        }
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
        if (recipeIconObject != null) recipeIconObject.SetActive(false);
        OrderCoffee();
    }

    void DeactivateCustomer()
    {
        satisfactionLevel = 100f;
        Destroy(gameObject, 2f);
    }
}