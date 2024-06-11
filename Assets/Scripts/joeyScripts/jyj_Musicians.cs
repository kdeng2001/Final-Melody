using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static jyj_entityData;

public class jyj_Musicians : MonoBehaviour
{
    [SerializeField] public Musician[] musicians; //list of all the musicians

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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

        return index;
    }
}
