using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    private void Start()
    {
        transform.position = new Vector3(target.position.x, transform.position.y + target.position.y, transform.position.z + target.position.z);
        transform.parent = target;
    }
}
