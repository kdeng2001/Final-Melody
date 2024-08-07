using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Musician", menuName = "Musician/Create new musician")]
public class Bo_MusicianBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] Sprite battleSprite;

    //Base Stats 
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;


    public string Name => name;
    public Sprite BattleSprite => battleSprite;
    public int MaxHP => maxHP;
    public int Attack => attack;
    public int Defense => defense;
    public int Speed => speed;

}