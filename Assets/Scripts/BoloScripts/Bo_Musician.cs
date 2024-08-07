using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]
public class Bo_Musician
{
    [SerializeField] Bo_MusicianBase _base;
    [SerializeField] int _level;

   

    public Bo_MusicianBase Base => _base; 
    public int Level => _level;

    public int HP { get; set; }
    public int MaxHp { get; private set; }

    public Bo_Musician(Bo_MusicianBase pBase, int pLevel)
    {
        _base = pBase;
        _level = pLevel;

        Init();
    }

    public void Init()
    {
        HP = MaxHp = _base.MaxHP;
    }

}
