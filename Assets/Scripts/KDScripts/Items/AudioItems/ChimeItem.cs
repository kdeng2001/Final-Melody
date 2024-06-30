using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeItem : Item
{
    public override void UseItem()
    {
        if(isSoundPlaying) { StopSFX(true); }
        else { PlaySFX(); }
        
    }
}
