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
        if (enemy != null)
        {
            maxHealth = enemy.m_health;
            UpdateBar();
        }
    }

    void Update()
    {
        UpdateBar();
        FaceCamera();
    }

    void UpdateBar()
    {
        if (enemy == null || fillBar == null) return;

        // maxHealth güncelle (eðer can deðiþtiyse)
        if (enemy.m_health > maxHealth)
            maxHealth = enemy.m_health;

        // Can oranýný hesapla (0.0 - 1.0)
        float fillAmount = maxHealth > 0 ? (float)enemy.m_health / maxHealth : 0f;
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

        // Billboard efekti - Canvas her zaman kameraya dönsün
        Vector3 direction = cameraTransform.forward;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

