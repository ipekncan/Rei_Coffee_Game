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

    private float currentTimeInSeconds;

    [Header("UI References")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public Image dayIcon; 
    public Sprite Sun; 
    public Sprite Moon;
    public Material Morning;
    public Material Afternoon;
    public Material Night;

    private int dayCount = 1;

    void Start()
    {
       
        currentTimeInSeconds = startHour * 3600;

        UpdateDayIcon();
    }

    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        currentTimeInSeconds += Time.deltaTime * timeMultiplier;
        if (currentTimeInSeconds >= 86400)
        {
            currentTimeInSeconds = 0;
            dayCount++;
            dayText.text = "Day " + dayCount;
        }

        DisplayTime();

        UpdateDayIcon();
        UpdateSkybox();
    }

    void DisplayTime()
    {
        int hours = Mathf.FloorToInt(currentTimeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((currentTimeInSeconds % 3600) / 60);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    void UpdateDayIcon()
    {
        float currentHour = GetCurrentHour();

        if (currentHour >= nightStartHour || currentHour < dayStartHour)
        {
            dayIcon.sprite = Moon;
        }
        else
        {
            dayIcon.sprite = Sun;
        }
    }

    void UpdateSkybox()
    {
        float currentHour = GetCurrentHour();
        if (currentHour >= nightStartHour || currentHour < dayStartHour)
        {
            RenderSettings.skybox = Night;
        }
        else if (currentHour >= dayStartHour && currentHour < (dayStartHour + nightStartHour) / 2)
        {
            RenderSettings.skybox = Morning;
        }
        else
        {
            RenderSettings.skybox = Afternoon;
        }
    }

    public float GetCurrentHour()
    {
        return currentTimeInSeconds / 3600;
    }
}