using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_entityData;
using static jyj_moves;

public class jyj_Musicians : MonoBehaviour
{
    [SerializeField] public Musician[] musicians; //list of all the musicians
    [SerializeField] private jyj_moves moveDatabase; //list of all moves

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * @brief recursive function that searches for a musician matching the name
     * @param name the name that is being searched for
     * @param index the current location in the musicians list
     * @param curr the current musician to be checked
     * @return the index of the matching musician name; returns -1 if it does not exist
     */
    public int getMusicianByName(string name, int index, Musician curr)
    {
        if (index >= musicians.Length)
        {
            return -1;
        }

        if (!name.Equals(curr.name))
        {
            return getMusicianByName(name, ++index, musicians[index]); //yes I know this is bad practice, but I don't want to make a linked list right now
        }

        //Don't find moves if it already has moves
        if (curr.moves.Count > 0)
        {
            return index;
        }

        for (int bogus = 0; bogus < curr.moveNames.Length; bogus++)
        {
            getMoveByName(curr.moveNames[bogus], index);
        }

        return index;
    }

    private void getMoveByName(string name, int location)
    {
        int index;

        if (moveDatabase.moves.Count <= 0)
        {
            Debug.Log("No move database");
            return;
        }

        index = moveDatabase.getMoveByName(name, 0, moveDatabase.moves[0]);

        if (index < 0)
        {
            Debug.Log("Musician not found");
            return;
        }

        //musician.moves.Add(moveDatabase.moves[index]);
        musicians[location].moves.Add(moveDatabase.moves[index]);
    }
}
