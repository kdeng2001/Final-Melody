using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_Musicians;
using UnityEngine.UIElements;

public class jyj_battleScene : MonoBehaviour
{
    private BattleParty player, enemy;
    [SerializeField] string[] playerPartyNames, enemyPartyNames;
    [SerializeField] jyj_Musicians database;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject precisionTimerObject;
    [SerializeField] private GameObject command;
    private bool init = false;
    public bool currentAction = false;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    command.transform.parent.gameObject.SetActive(false);
    //    command.SetActive(false);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !init)
        {
            init = true;
            command.transform.parent.gameObject.SetActive(true);
            player = new BattleParty(playerPartyNames, PartyType.PARTY_TYPE_PLAYER, ref database);
            enemy = new BattleParty(enemyPartyNames, PartyType.PARTY_TYPE_ENEMY, ref database);

            for (int bogus = 0; bogus < player.party.musicians[0].moves.Count; bogus++)
            {
                Debug.Log("running " + (bogus + 1) + " out of " + player.party.musicians[0].moves.Count);
                /*string name = player.party.musicians[0].moves[bogus].name;

                if (name.Equals("drumroll"))
                {
                    player.party.musicians[0].moves[bogus].action.timer = Instantiate(timerObject);
                }
                else if (name.Equals("kick"))
                {
                    player.party.musicians[0].moves[bogus].action.timer = Instantiate(precisionTimerObject);
                }
                else
                {
                    Debug.Log("No timer needed");
                }*/

                
                switch (player.party.musicians[0].moves[bogus].name)
                {
                    case "drumroll":
                        player.party.musicians[0].moves[bogus].action.timer = Instantiate(timerObject, transform);
                        break;
                    case "kick":
                        player.party.musicians[0].moves[bogus].action.timer = Instantiate(precisionTimerObject, transform);
                        break;
                    default:
                        Debug.Log("No timer needed");
                        break;
                }
                

                player.party.musicians[0].moves[bogus].action.command = command;

                if (player.party.musicians[0].moves[bogus].action.timer == null)
                {
                    continue;
                }

                player.party.musicians[0].moves[bogus].action.timer.SetActive(false);
                //player.party.musicians[0].moves[bogus].action.command.SetActive(false);
            }
        }
    }

    public void onMoveClick(GameObject button)
    {
        if (!init)
        {
            button.SetActive(true);
            return;
        }

        command.SetActive(true);
        //currentAction = true;
        //button.GetComponent<Button>().SetEnabled(false);

        for (int bogus = 0; bogus < player.party.musicians[0].moves.Count; bogus++)
        {
            if (button.ToString().Contains(player.party.musicians[0].moves[bogus].name))
            {
                if (player.party.musicians[0].moves[bogus].action.timer == null)
                {
                    switch (player.party.musicians[0].moves[bogus].name)
                    {
                        case "drumroll":
                            player.party.musicians[0].moves[bogus].action.timer = Instantiate(timerObject, transform);
                            break;
                        case "kick":
                            player.party.musicians[0].moves[bogus].action.timer = Instantiate(precisionTimerObject, transform);
                            break;
                        default:
                            Debug.Log("No timer needed");
                            break;
                    }

                    if (player.party.musicians[0].moves[bogus].action.timer != null)
                    {
                        player.party.musicians[0].moves[bogus].action.timer.SetActive(false);
                    }
                }

                player.party.musicians[0].moves[bogus].action.button = button;
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