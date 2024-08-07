using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bo_BattleSystem : MonoBehaviour
{
    public StateMachine<Bo_BattleSystem> StateMachine { get; private set; }

    [SerializeField] Bo_Unit playerUnit;
    [SerializeField] Bo_Party playerParty;

    [SerializeField] Bo_Unit enemyUnit;
    [SerializeField] Bo_Party enemyParty;

    //Preferebly in the future, battle system is started and updated by game manager? 
    void Start()
    {
       // GameObject player = GameObject.FindWithTag("Player");
      //  playerParty = player.GetComponent<Bo_Party>();

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        StateMachine = new StateMachine<Bo_BattleSystem>(this);


        playerUnit.Setup(playerParty.Musicians[0]);
        enemyUnit.Setup(enemyParty.Musicians[0]);

        yield return new WaitForSeconds(1f);
        Debug.Log("Battle START");

    }

    void Update()
    {

        if (StateMachine.CurrentState != null)
            StateMachine.Execute();
    }
}
