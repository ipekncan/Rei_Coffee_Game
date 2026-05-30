using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomersController : MonoBehaviour
{
    public bool isActive = true;
    public bool isWaiting = false;

    [SerializeField] private Transform waitPlace;

    private NavMeshAgent agent;
    private Animator animator;
    private bool hasReachedDestination = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();

        if (isActive && waitPlace != null)
            StartCoroutine(GoToWaitPlaceDelayed());
    }

    private IEnumerator GoToWaitPlaceDelayed()
    {
        // NavMesh'in hazýr olmasý için bir frame bekle
        yield return null;

        // Agent NavMesh üzerinde mi kontrol et
        if (agent.isOnNavMesh)
        {
            GoToWaitPlace();
        }
        else
        {
            Debug.LogWarning("Agent NavMesh üzerinde deðil: " + gameObject.name);
        }
    }

    void Update()
    {
        if (!isActive || isWaiting) return;
        if (agent.pathPending) return;

        // Geçerli bir path yoksa kontrol etme
        if (!agent.hasPath) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
            if (agent.velocity.sqrMagnitude < 0.01f)
                StartWaiting();
    }

    private void GoToWaitPlace()
    {
        // Hedefe gitmeyi baþlat
        agent.isStopped = false;
        agent.SetDestination(waitPlace.position);
        hasReachedDestination = false;

        // Walk animasyonunu baþlat
        animator.SetBool("isStopped", false);
    }

    private void StartWaiting()
    {
        isWaiting = true;
        hasReachedDestination = true;

        // Bekleme pozisyonuna geldi, hareketi durdur
        agent.isStopped = true;

        // Idle animasyonuna geç
        animator.SetBool("isStopped", true);

        Debug.Log("Müþteri bekleme noktasýna geldi: " + gameObject.name);
    }

    // Müþteriyi yeniden aktif etmek için bir metod
    public void ActivateCustomer()
    {
        isActive = true;
        isWaiting = false;
        hasReachedDestination = false;

        if (waitPlace != null)
        {
            GoToWaitPlace();
        }
    }

    // Müþteriyi devre dýþý býrakmak için bir metod
    public void DeactivateCustomer()
    {
        isActive = false;
        isWaiting = false;
        agent.isStopped = true;
        animator.SetBool("isStopped", true);
    }

    // Görsel hata ayýklama için (isteðe baðlý)
    void OnDrawGizmosSelected()
    {
        if (waitPlace != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(waitPlace.position, 0.5f);

            if (agent != null && agent.hasPath)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, agent.destination);
            }
        }
    }
}