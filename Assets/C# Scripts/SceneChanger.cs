using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad; // Name of the scene to load
    public string targetSpawnID;

    public static string lastTargetSpawnID="";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lastTargetSpawnID = targetSpawnID;
            Debug.Log("Hafýzaya atanan target id: " + lastTargetSpawnID);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }

}
