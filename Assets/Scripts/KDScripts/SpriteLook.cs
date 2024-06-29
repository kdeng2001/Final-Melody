using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLook : MonoBehaviour
{
    [SerializeField] private Transform lookAtObj;
    [SerializeField] private bool ignoreY = false;
    private void Update()
    {
        //transform.LookAt(Camera.main.transform);
        if(Camera.main == null) { return; }
        Vector3 newtarget;

        if (lookAtObj != null) { newtarget = lookAtObj.position; } 
        else { newtarget = Camera.main.transform.position; }

        if(ignoreY)
        {
            newtarget.x = transform.position.x;
        }

        transform.LookAt(newtarget);

    }
}
