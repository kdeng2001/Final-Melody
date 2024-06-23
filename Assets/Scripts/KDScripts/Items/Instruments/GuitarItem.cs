using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarItem : Item, IInstrument
{
    public void Play()
    {
        throw new System.NotImplementedException();
    }

    public override void UseItem()
    {
        //Debug.Log("use guitar!");
        return;
    }
}
