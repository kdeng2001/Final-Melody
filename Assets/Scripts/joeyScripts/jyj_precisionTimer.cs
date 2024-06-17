using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jyj_precisionTimer : jyj_timer
{
    [SerializeField] private float precisionThreshold;
    [SerializeField] private float endThreshold;

    void Update()
    {
        if (Time.time - time > target)
        {
            Debug.Log("Time Up!");
            move.endAction();
            Destroy(this);
        }

        if (Time.time - time > precisionThreshold && Time.time - time < endThreshold)
        {
            Debug.Log("Now!");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                move.mult++;
                Debug.Log("Good!");
                target = -1;
            }
        }
    }
}
