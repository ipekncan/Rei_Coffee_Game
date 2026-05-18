using UnityEngine;


public class SceneChanger : MonoBehaviour
{

    private TimeManager timeManager;

    [Header("Settings")]
    public string sceneToLoad; // Name of the scene to load
    public string targetSpawnID;

    public static string lastTargetSpawnID = "";


    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (timeManager == null)
        {
            Debug.LogError("TimeManager bulunamadý! Lütfen sahnede bir TimeManager olduðundan emin olun.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float currentHour = timeManager.GetCurrentHour();
        if (other.CompareTag("Player"))
        {

            if (sceneToLoad == "Backyard" || sceneToLoad == "Forest")
            {

                if (currentHour >= timeManager.dayStartHour && currentHour < timeManager.nightStartHour)
                {
                    Debug.Log("Dýþarýsý çok tehlikeli! Sadece gece vakti (22:00'den sonra) çýkabilirsin.");
                    
                    return;
                }
                lastTargetSpawnID = targetSpawnID;
                Debug.Log("Hafizaya atanan target id: " + lastTargetSpawnID);
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            }
        }

    }
}
