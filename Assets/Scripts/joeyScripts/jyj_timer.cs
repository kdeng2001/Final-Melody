using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_moveActions;

public class jyj_timer : MonoBehaviour
{
    private float time;
    [SerializeField] private float target = 5;
    private MoveAction move;

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
