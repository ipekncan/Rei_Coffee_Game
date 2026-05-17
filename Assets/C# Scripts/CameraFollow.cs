using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow
    public Vector3 offset = new Vector3(10f, 10f, -10f);
    public float smoothSpeed = 5f;
    void Start()
    {
        GameObject player=GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Kamera takip edecek 'Player' tag'li bir obje bulamadý!");
        }
    }

    //Not:kamera takibini Late Update'e koymamýzýn sebebi, karakterin hareketini Update'de yaparken kameranýn hareketini Late Update'de yaparak, karakterin hareketi tamamlandýktan sonra kameranýn pozisyonunu güncellemesini saðlamaktýr. Bu sayede kamera, karakterin hareketine daha düzgün ve akýcý bir þekilde tepki verebilir.
    void LateUpdate()
    {
        if (target == null) { return; }
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
