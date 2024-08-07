using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bo_Party : MonoBehaviour
{
    [SerializeField] List<Bo_Musician> musicians;
    public List<Bo_Musician> Musicians { get { return musicians; } set { musicians = value;} }

    public void AddCharacter(Bo_Musician newChar)
    {
        if (musicians.Count < 4)
        {
            musicians.Add(newChar);
        }
    }
}
