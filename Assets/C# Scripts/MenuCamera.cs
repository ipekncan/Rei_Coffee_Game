using UnityEngine;
using Unity.Cinemachine;

public class MenuCamera : MonoBehaviour
{
    private CinemachineSplineDolly splinedolly;
    [Header("Cinemachine Settings")]
    public float cameraSpeed = 0.02f;

    void Start()
    {
        splinedolly = GetComponent<CinemachineSplineDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (splinedolly != null)
        {
            splinedolly.CameraPosition += cameraSpeed * Time.deltaTime;
            if(splinedolly.CameraPosition > 1f)
            {
                splinedolly.CameraPosition = 0f; // Loop back to the start of the paththe camera is moving in a circle
            }
        }
    }
}
