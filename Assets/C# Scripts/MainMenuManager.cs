using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Object StartScene;

        public void StartGame()
     {
        if(StartScene == null)
        {
            Debug.LogError("StartScene is not assigned in the inspector.");
            return;
        }
        else 
        { 
            SceneManager.LoadScene(StartScene.name);
        }
        
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

    }
}
