using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumsItem : Item, IInstrument
{
    public void Play()
    {
        throw new System.NotImplementedException();
    }

    public override void UseItem()
    {
        //Debug.Log("use drums!");
        return;
    }
}
