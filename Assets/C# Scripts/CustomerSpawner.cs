using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform customerTarget; // müþterinin yürüyeceði hedef

    private bool isWaitingToSpawn = false;

    void Start()
    {
        SpawnCustomer();
    }

    void Update()
    {
        // Sahnede müþteri kalmadýysa ve bekleme baþlamadýysa yeni spawn planla
        if (!isWaitingToSpawn && FindFirstObjectByType<CustomerController>() == null)
        {
            isWaitingToSpawn = true;
            float delay = Random.Range(3f, 10f);
            Invoke(nameof(SpawnCustomer), delay);
        }
    }

    void SpawnCustomer()
    {
        if (customerPrefab == null || spawnPoint == null) return;

        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Target'ý ata
        if (customerTarget != null)
            newCustomer.GetComponent<CustomerController>().target = customerTarget;

        isWaitingToSpawn = false;
    }
}