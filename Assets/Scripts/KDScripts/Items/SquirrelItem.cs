using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelItem : Item
{
    public override void UseItem()
    {
        // perform item behavior...
        Debug.Log("using " + itemName);
        // decrease amount if item is not reusable
        if (reusable) { return; }
        InventoryUI.Instance.inventory.UpdateItem(itemName, -1);
    }
}