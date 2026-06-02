using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; 

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel;

    [Header("UI Elements")]
    public Slider volumeSlider;

   
    [Header("Input")]
    public InputActionReference pauseAction;

    private bool isPaused = false;

    void Awake()
    {
        pauseAction.action.performed += OnPausePerformed;
    }

    void OnDestroy()
    {
        
        pauseAction.action.performed -= OnPausePerformed;
    }

    void Start()
    {
        optionsPanel.SetActive(false);
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;
        optionsPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void CloseMenu()
    {
        isPaused = false;
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}