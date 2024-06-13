using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * @purpose handles entity data that will be used during a battle
 */
public class jyj_entityData : MonoBehaviour
{
    [Serializable]
    public struct Party
    {
        private Musician curr; //current musician on the field
        public int partySize; //number of total musicians
        public List<Musician> musicians; //the musicians in the party; order does not represent who is on the field
        public PartyType partyType; //the type of party this is
    }

    [Serializable]
    public struct Musician
    {
        public string name; //name of the musician
        public int health; //current health
        public int maxHealth; //max health
        public int moveSize; //number of moves
        public List<Move> moves; //moves the musician has access to
        private Move curr; //current move to be used; keep this move selected as default for next turn
        public Type type; //type of the musician
        public string[] moveNames; //the names of the moves; use this to search the move database
    }

    [Serializable]
    public struct Move
    {
        public string name; //move name
        public string description; //move description
        public Type type; //type of the move
        public int power; //base attack power of the move
        public MinigameType minigame; //type of minigame the move uses
    }

    [Serializable]
    public enum Type
    {
        TYPE_NONE
    }

    [Serializable]
    public enum MinigameType
    {
        MINIGAME_TYPE_NONE
    }

    [Serializable]
    public enum PartyType
    {
        PARTY_TYPE_NONE,
        PARTY_TYPE_PLAYER,
        PARTY_TYPE_ENEMY,
        PARTY_TYPE_ALLY
    }

    [SerializeField] private Party party;
    [SerializeField] private string[] musicianNames;
    [SerializeField] private jyj_Musicians musicianDatabase;

    // Start is called before the first frame update
    /*
     * @brief populates the player's party with musicians
     */
    void Start()
    {
        if (musicianNames.Length <= 0)
        {
            Debug.Log("No musician names given");
            return;
        }

        //party.musicians = new Musician[musicianNames.Length];

        for (int bogus = 0; bogus < musicianNames.Length; bogus++)
        {
            getMusicianByName(musicianNames[bogus]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * @brief finds the index of the musician from the musician database
     * @param name the name of the musician
     */
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
    }
}
