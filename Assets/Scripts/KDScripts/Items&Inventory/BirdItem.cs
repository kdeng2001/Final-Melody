using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdItem : Item
{
    public override void UseItem()
    {
        // perform item behavior...
        // decrease amount if item is not reusable
        if (reusable) { return; }
        InventoryUI.Instance.inventory.UpdateItem(itemName, -1);
    }
}
