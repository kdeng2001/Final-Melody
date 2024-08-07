using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bo_Unit : MonoBehaviour
{
    [HideInInspector]
    protected SpriteRenderer sprRender;

    [field: SerializeField]
    public Bo_Musician Character { get; set; }

    public SpriteRenderer SprRenderer { get { return sprRender; } }
    public virtual void Setup(Bo_Musician character)
    {
        Character = character;
        sprRender = GetComponentInChildren<SpriteRenderer>();

        sprRender.sprite = Character.Base.BattleSprite;
    }


}
