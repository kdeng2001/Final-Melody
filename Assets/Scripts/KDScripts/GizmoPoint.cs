using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoPoint : MonoBehaviour
{
    public float radius = .5f;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
