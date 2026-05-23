using TMPro;
using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour
{

    private TimeManager timeManager;

    [Header("Settings")]
    public string sceneToLoad; // Name of the scene to load
    public string targetSpawnID;


    [Header("Warning UI")]
    public GameObject warningPanel; 
    public TextMeshProUGUI warningText; 
    public float displayDuration = 3f;

    public static string lastTargetSpawnID = "";


    

    private void Start()
    {

        timeManager = FindFirstObjectByType<TimeManager>();
        if (timeManager == null)
        {
            Debug.LogError("TimeManager bulunamadý!");
        }

        if (warningPanel != null) warningPanel.SetActive(false);
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
                    ShowWarning("Dýþarýsý þu an çok tehlikeli, geceyi bekle!");
                    return;

                }
                
            }
            lastTargetSpawnID = targetSpawnID;
            Debug.Log("Hafizaya atanan target id: " + lastTargetSpawnID);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }

        void ShowWarning(string message)
        {
            if (warningPanel != null && warningText != null)
            {
                StopAllCoroutines(); // Eðer üst üste binerse öncekini durdur
                StartCoroutine(WarningRoutine(message));
            }
        }

        IEnumerator WarningRoutine(string message)
        {
            warningText.text = message;
            warningPanel.SetActive(true);

            yield return new WaitForSeconds(displayDuration);

            warningPanel.SetActive(false);
        }
    }
}
