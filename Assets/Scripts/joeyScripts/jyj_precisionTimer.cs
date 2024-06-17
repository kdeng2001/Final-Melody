using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jyj_precisionTimer : jyj_timer
{
    [SerializeField] private float precisionThreshold;
    [SerializeField] private float endThreshold;
    public bool isCustom = false;
    private KeyCode[] customInputs;

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

            if (Input.GetKeyDown(KeyCode.Space) && !isCustom)
            {
                move.mult++;
                Debug.Log("Good!");
                target = -1;
            }
            else if (isCustom)
            {
                for (int bogus = 0; bogus < customInputs.Length; bogus++)
                {
                    if (Input.GetKeyDown(customInputs[bogus]))
                    {
                        move.mult++;
                        Debug.Log("Good!");
                        target = -1;
                    }
                }
            }
        }
    }

    public void setCustom(KeyCode[] inputs)
    {
        customInputs = new KeyCode[inputs.Length];

        for (int bogus = 0; bogus < inputs.Length; bogus++)
        {
            customInputs[bogus] = inputs[bogus];
        }

        //customInputs = inputs;
        isCustom = true;
    }
}
