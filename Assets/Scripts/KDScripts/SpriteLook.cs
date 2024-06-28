using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLook : MonoBehaviour
{
    [SerializeField] private bool ignoreY = false;
    private void Update()
    {
        //transform.LookAt(Camera.main.transform);
        if(Camera.main == null) { return; }
        Vector3 newtarget = Camera.main.transform.position;
        if(ignoreY)
        {
            newtarget.x = transform.position.x;
        }

        transform.LookAt(newtarget);

    }
}
