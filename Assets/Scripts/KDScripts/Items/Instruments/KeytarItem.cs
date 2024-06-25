using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeytarItem : Item, IInstrument
{
    public void Play()
    {
        throw new System.NotImplementedException();
    }

    public override void UseItem()
    {
        //Debug.Log("use keytar!");
        return;
    }
}
