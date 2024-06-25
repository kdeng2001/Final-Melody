using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Billboard_SC : MonoBehaviour
{
    private Transform cameraTransform;
    public Camera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = gameCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
        transform.rotation = quaternion.Euler(0, 0, 0);
    }
}
