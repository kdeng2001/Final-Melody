using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_Musicians;

public class jyj_battleScene : MonoBehaviour
{
    private BattleParty player, enemy;
    [SerializeField] string[] playerPartyNames, enemyPartyNames;
    [SerializeField] jyj_Musicians database;
    [SerializeField] private GameObject timerObject;
    private bool init = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !init)
        {
            init = true;
            player = new BattleParty(playerPartyNames, PartyType.PARTY_TYPE_PLAYER, ref database);
            enemy = new BattleParty(enemyPartyNames, PartyType.PARTY_TYPE_ENEMY, ref database);

            player.party.musicians[0].moves[0].action.timer = Instantiate(timerObject);
            player.party.musicians[0].moves[0].action.timer.SetActive(false);
            //player.party.musicians[0].moves[0].action.moveAction();
        }
    }

    public void onMoveClick(string moveName)
    {
        if (!init)
        {
            return;
        }

        for (int bogus = 0; bogus < player.party.musicians[0].moves.Count; bogus++)
        {
            if (player.party.musicians[0].moves[bogus].name.Equals(moveName))
            {
                player.party.musicians[0].moves[bogus].action.moveAction();
            }
        }
    }
}

public class BattleParty
{
    public Party party;
    private jyj_Musicians musicianDatabase;


    public BattleParty(string[] names, PartyType type, ref jyj_Musicians database)
    {
        party.partyType = type;
        musicianDatabase = database;

        if (names.Length <= 0)
        {
            Debug.Log("No musician names given");
            return;
        }

        party.musicians = new List<Musician>();

        for (int bogus = 0; bogus < names.Length; bogus++)
        {
            getMusicianByName(names[bogus]);
        }
    }
    private void getMusicianByName(string name)
    {
        int index;
        //Musician[] temp;

        if (musicianDatabase.musicians.Length <= 0)
        {
            Debug.Log("No musician database");
            return;
        }

        index = musicianDatabase.getMusicianByName(name, 0, musicianDatabase.musicians[0]);

        if (index < 0)
        {
            Debug.Log("Musician not found");
            return;
        }

        party.musicians.Add(musicianDatabase.musicians[index]);
        Debug.Log("Musician found successfully");
    }
}