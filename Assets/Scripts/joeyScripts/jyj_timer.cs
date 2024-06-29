using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_moveActions;
using TMPro;
using UnityEngine.UIElements;

public class jyj_timer : MonoBehaviour
{
    protected float time;
    [SerializeField] protected float target = 5;
    protected MoveAction move;
    protected TextMeshProUGUI text;

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
            //this.gameObject.SetActive(false);
            move.timer = null;
            GetComponentInParent<jyj_battleScene>().currentAction = false;
            move.button.SetActive(true);
            Destroy(this);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            move.mult++;
            Debug.Log("Good!");
        }
    }

    public void setMoveAction(MoveAction action, TextMeshProUGUI text)
    {
        move = action;
        time = Time.time;
        this.text = text;
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
