using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_entityData;
using static jyj_timer;
using System;
using TMPro;

public class jyj_moveActions : MonoBehaviour
{
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject precisionTimerObject;

    
    // Start is called before the first frame update
    void Start()
    {

        /*Drumroll drum = new Drumroll();
        drum.timer = Instantiate(timerObject);
        drum.timer.SetActive(false);
        drum.moveAction();*/

        /*Kick kick = new Kick();
        kick.timer = Instantiate(precisionTimerObject);
        kick.timer.SetActive(false);
        kick.moveAction();*/

        /*Solo solo = new Solo();
        solo.moveAction();*/

        //Encouragement encourage = new Encouragement();
        //encourage.moveAction();

        /*TripleChord chord = new TripleChord();
        chord.timer = Instantiate(precisionTimerObject);
        chord.timer.SetActive(false);
        chord.moveAction();*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public abstract class MoveAction
{
    protected Move moveData;
    public GameObject timer;
    public float mult;
    public string name;
    public GameObject command;
    public GameObject button;

    public abstract void moveAction();
    public abstract void endAction();
}

public class Drumroll : MoveAction
{
    private int damage;
    //public int mult;

    public Drumroll()
    {
        moveData.power = 10;
        name = "drumroll";
    }

    public override void moveAction()
    {
        mult = 0;
        Debug.Log("Press 'SPACE' as much as you can!");
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Press 'SPACE' as much as you can!";
        timer.GetComponent<jyj_timer>().setMoveAction(this, text);
        timer.SetActive(true);
        //float time = Time.time;


        /*while (Time.time - time < 5)
        {
            Debug.Log(Time.time - time);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mult++;
                Debug.Log("Good!");
            }

            //time -= Time.deltaTime;
            //yield return null;
        }*/

        //StartCoroutine(check());
    }

    public override void endAction()
    {
        //Debug.Log("Times up!");
        damage = (int)(moveData.power * mult);
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Damage dealt: " + damage;
        Debug.Log("Damage dealt: " + damage);
        button.SetActive(true);
    }

    private IEnumerator check()
    {
        Debug.Log("Starting Coroutine");
        float time = Time.time;

        while (Time.time - time < 5)
        {
            Debug.Log(Time.time - time);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mult++;
                Debug.Log("Good!");
            }

            time -= Time.deltaTime;
            yield return null;
        }
    }
}

public class Kick : MoveAction
{
    private int damage;

    public Kick()
    {
        moveData.power = 10;
        name = "kick";
    }
    public override void moveAction()
    {
        mult = 0;
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Press space at the right time!";
        Debug.Log("Press space at the right time!");
        timer.GetComponent<jyj_precisionTimer>().setMoveAction(this, text);
        timer.SetActive(true);
    }

    public override void endAction()
    {
        //Debug.Log("Times up!");
        damage = (int)(moveData.power * mult);
        Debug.Log("Damage dealt: " + damage);
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Damage dealt: " + damage;
        button.SetActive(true);
    }
}

public class Solo : MoveAction
{
    private int damage;

    public Solo()
    {
        moveData.power = 10;
        name = "solo";
    }

    public override void moveAction()
    {
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Extra power!";
        Debug.Log("Extra power!");
        mult = 1.2f;
        endAction();
    }

    public override void endAction()
    {
        damage = (int)(moveData.power * mult);
        Debug.Log("Damage dealt: " + damage);
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Damage dealt: " + damage;
        button.SetActive(true);
    }
}

public class Encouragement : MoveAction
{
    private int heal;

    public Encouragement()
    {
        moveData.power = 10;
        name = "encouragement";
    }

    public override void moveAction()
    {
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Encouragement Time!";
        Debug.Log("Healing time!");
        mult = 1;
        endAction();
    }

    public override void endAction()
    {
        heal = (int)(moveData.power * mult);
        Debug.Log("Healed: " + heal);
        TextMeshProUGUI text = command.GetComponent<TextMeshProUGUI>();
        text.text = "Healed: " + heal;
        button.SetActive(true);
    }
}

public class TripleChord : MoveAction
{
    private int damage;
    private KeyCode[] inputs;

    public TripleChord()
    {
        moveData.power = 10;
        name = "triple chord";
    }

    public override void moveAction()
    {
        System.Random rand = new System.Random();
        inputs = new KeyCode[3];
        int num = rand.Next(0, 3);

        switch (num)
        {
            case 0:
                inputs[0] = KeyCode.Q;
                inputs[1] = KeyCode.W;
                inputs[2] = KeyCode.E;
                Debug.Log("Press Q, W, and E when prompted!");
                break;
            case 1:
                inputs[0] = KeyCode.A;
                inputs[1] = KeyCode.S;
                inputs[2] = KeyCode.D;
                Debug.Log("Press A, S, and D when prompted!");
                break;
            case 2:
                inputs[0] = KeyCode.Z;
                inputs[1] = KeyCode.X;
                inputs[2] = KeyCode.C;
                Debug.Log("Press Z, X, and C when prompted");
                break;
            default:
                Debug.Log("Unexpected error occured");
                break;
        }

        timer.GetComponent<jyj_precisionTimer>().setCustom(inputs);
        timer.GetComponent<jyj_precisionTimer>().setMoveAction(this, null);
        timer.SetActive(true);
    }

    public override void endAction()
    {
        damage = (int)(mult * moveData.power);
        Debug.Log("Damage dealt: " + damage);
    }
}
