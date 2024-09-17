using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Give2000MoneyItem : Item
{
    public override void UseItem()
    {
        InventoryUI.Instance.points.UpdateMoney(2000);
        if (reusable) { return; }
        InventoryUI.Instance.inventory.UpdateItem(itemName, -1);
    }
}
