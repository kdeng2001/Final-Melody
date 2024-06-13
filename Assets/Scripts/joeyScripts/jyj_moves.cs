using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_entityData;

public class jyj_moves : MonoBehaviour
{
    [SerializeField] public List<Move> moves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getMoveByName(string name, int index, Move curr)
    {
        if (index >= moves.Count)
        {
            return -1;
        }

        if (!name.Equals(curr.name))
        {
            return (getMoveByName(name, ++index, moves[index]));
        }

        return index;
    }
}
