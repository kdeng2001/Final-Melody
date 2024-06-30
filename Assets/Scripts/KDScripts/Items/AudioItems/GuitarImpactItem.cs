using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarImpactItem : Item
{
    public override void UseItem()
    {
        if (isSoundPlaying) { StopSFX(true); }
        else { PlaySFX(); }

    }
}