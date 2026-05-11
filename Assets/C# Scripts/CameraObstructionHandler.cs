using UnityEngine;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{
    public Transform target; 
    public LayerMask wallLayer; //Layer of the walls that can obstruct the view

    private List<MeshRenderer> lastObstructions = new List<MeshRenderer>();

    void Update()
    {
        
        Vector3 direction = target.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.position);

        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, wallLayer);

      
        ResetObstructions();

        foreach (var hit in hits)
        {
            MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                MakeTransparent(mesh);
                lastObstructions.Add(mesh);
            }
        }
    }

    void MakeTransparent(MeshRenderer mesh)
    {
        // Take material color and set alpha to 0.3f for transparency
        Color color = mesh.material.color;
        color.a = 0.3f;
        mesh.material.color = color;
    }

    void ResetObstructions()
    {
        foreach (var mesh in lastObstructions)
        {
            Color color = mesh.material.color;
            color.a = 1f; 
            mesh.material.color = color;
        }
        lastObstructions.Clear();
    }
}