using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Referanslar")]
    public Image fillBar;           // Yeþil dolum barý
    public EnemyController enemy;   // Düþman scripti

    [Header("Renk Ayarlarý")]
    public Color highHealthColor = Color.green;   // Tam can: yeþil
    public Color lowHealthColor = Color.red;       // Az can: kýrmýzý

    private int maxHealth;
    private Transform cameraTransform;

    void Start()
    {
        // Kamerayý bul (bar hep kameraya baksýn)
        cameraTransform = Camera.main.transform;

        // Düþman scripti bu objenin parent'ýnda aranýr
        if (enemy == null)
            enemy = GetComponentInParent<EnemyController>();

        // Baþlangýç canýný max olarak kaydet
        maxHealth = enemy.m_health;

        UpdateBar();
    }

    void Update()
    {
        UpdateBar();
        FaceCamera();
    }

    void UpdateBar()
    {
        if (enemy == null || fillBar == null) return;

        // Can oranýný hesapla (0.0 - 1.0)
        float fillAmount = (float)enemy.m_health / maxHealth;
        fillAmount = Mathf.Clamp01(fillAmount);

        // Barý güncelle
        fillBar.fillAmount = fillAmount;

        // Rengini deðiþtir: çok canlýysa yeþil, azalýnca kýrmýzýya kayar
        fillBar.color = Color.Lerp(lowHealthColor, highHealthColor, fillAmount);

        // Düþman öldüyse barý gizle
        if (enemy.isDead)
            gameObject.SetActive(false);
    }

    void FaceCamera()
    {
        if (cameraTransform == null) return;

        // Canvas her zaman kameraya dönsün
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}