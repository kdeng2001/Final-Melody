using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_moveActions;

public class jyj_timer : MonoBehaviour
{
    protected float time;
    [SerializeField] protected float target = 5;
    protected MoveAction move;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time > target)
        {
            Debug.Log("Time Up!");
            move.endAction();
            Destroy(this);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            move.mult++;
            Debug.Log("Good!");
        }
    }

    public void setMoveAction(MoveAction action)
    {
        move = action;
        time = Time.time;
    }
}

/*public class PrecisionTimer : jyj_timer
{
    private float precisionThreshold;

    void Update()
    {
        if (Time.time - time > target)
        {
            Debug.Log("Time Up!");
            move.endAction();
            Destroy(this);
        }

        if (Time.time - time > precisionThreshold + 1 && Time.time - time < precisionThreshold - 1 && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Now!");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                move.mult++;
                Debug.Log("Good!");
            }
        }
    }
}*/
