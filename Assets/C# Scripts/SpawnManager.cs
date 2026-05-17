using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        string targetID = SceneChanger.lastTargetSpawnID;
        Debug.Log("SpawnManager Çalýþtý. Hafýzadaki ID: " + targetID);

        if (string.IsNullOrEmpty(targetID))
        {
            Debug.Log("Hafýzadaki target ID yok, baþlangýç noktasýnda kalýnýyor.");
            return;
        }

        GameObject[] SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Sahnede 'Player' tag'li bir obje bulunamadý!");
            return;
        }

        foreach (GameObject spawnPoint in SpawnPoints)
        {
            if (spawnPoint.name == targetID)
            {
              
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;

                if (cc != null) cc.enabled = true;

                Debug.Log("Player baþarýyla ýþýnlandý: " + spawnPoint.name);

                
                return;
            }
        }

        Debug.LogWarning("Hafýzadaki ID ile eþleþen bir SpawnPoint objesi sahnede bulunamadý!");
    }
}