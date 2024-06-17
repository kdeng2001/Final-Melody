using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_entityData;
using static jyj_timer;

public class jyj_moveActions : MonoBehaviour
{
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject precisionTimerObject;

    public abstract class MoveAction
    {
        protected Move moveData;
        public GameObject timer;
        public int mult;

        public abstract void moveAction();
        public abstract void endAction();
    }

    private class Drumroll : MoveAction
    {
        private int damage;
        //public int mult;

        public Drumroll()
        {
            moveData.power = 10;

        }

        public override void moveAction()
        {
            Debug.Log("Press 'SPACE' as much as you can!");
            timer.GetComponent<jyj_timer>().setMoveAction(this);
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
            damage = moveData.power * mult;
            Debug.Log("Damage dealt: " + damage);
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

    private class Kick : MoveAction
    {
        private int damage;

        public Kick()
        {
            moveData.power = 10;
        }
        public override void  moveAction()
        {
            Debug.Log("Press space at the right time!");
            timer.GetComponent<jyj_precisionTimer>().setMoveAction(this);
            timer.SetActive(true);
        }

        public override void endAction()
        {
            //Debug.Log("Times up!");
            damage = moveData.power * mult;
            Debug.Log("Damage dealt: " + damage);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        /*Drumroll drum = new Drumroll();
        drum.timer = Instantiate(timerObject);
        drum.timer.SetActive(false);
        drum.moveAction();*/

        Kick kick = new Kick();
        kick.timer = Instantiate(precisionTimerObject);
        kick.timer.SetActive(false);
        kick.moveAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
