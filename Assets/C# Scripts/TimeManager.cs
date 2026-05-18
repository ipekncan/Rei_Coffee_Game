using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeMultiplier = 200f;
    public float startHour = 8f;
    public float nightStartHour = 22f;
    public float dayStartHour = 8f;


    private static float currentTimeInSeconds = -1f;
    private static int dayCount = 1;

    [Header("UI References")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public Image dayIcon;
    public Sprite Sun;
    public Sprite Moon;

    [Header("Skybox Materials")]
    public Material Morning;
    public Material Afternoon;
    public Material Night;

    void Start()
    {
     
        if (currentTimeInSeconds < 0)
        {
            currentTimeInSeconds = startHour * 3600;
        }

        if (dayText != null) dayText.text = "Day " + dayCount;

        UpdateDayIcon();
        UpdateSkybox();
    }

    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        currentTimeInSeconds += Time.deltaTime * timeMultiplier;

        if (currentTimeInSeconds >= (24*3600))
        {
            currentTimeInSeconds = 0;
            dayCount++;
            if (dayText != null) dayText.text = "Day " + dayCount;
        }

        DisplayTime();
        UpdateDayIcon();
        UpdateSkybox();
    }

    void DisplayTime()
    {
        if (timeText == null) return;
        int hours = Mathf.FloorToInt(currentTimeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((currentTimeInSeconds % 3600) / 60);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    void UpdateDayIcon()
    {
        if (dayIcon == null) return;
        float currentHour = GetCurrentHour();

        if (currentHour >= nightStartHour || currentHour < dayStartHour)
            dayIcon.sprite = Moon;
        else
            dayIcon.sprite = Sun;
    }

    void UpdateSkybox()
    {
        float currentHour = GetCurrentHour();
        if (currentHour >= nightStartHour || currentHour < dayStartHour)
            RenderSettings.skybox = Night;
        else if (currentHour >= dayStartHour && currentHour < 15f)
            RenderSettings.skybox = Morning;
        else
            RenderSettings.skybox = Afternoon;
    }

    public float GetCurrentHour()
    {
        return currentTimeInSeconds / 3600;
    }
}