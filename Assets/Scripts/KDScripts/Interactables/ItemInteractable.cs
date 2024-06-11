using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] Item item;
    public delegate void ItemWasObtained(string name, int amount, string iconPath);
    public ItemWasObtained itemWasObtained;
    public override void OnFinishInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStartInteract()
    {
        itemWasObtained?.Invoke(item.itemName, item.amount, item.iconFilePath);
        item.HandleObtained();
    }

    public override void Start()
    {
        base.Start();
        itemWasObtained += InventoryUI.Instance.inventory.UpdateItem;
    }
    private void OnDestroy()
    {
        itemWasObtained -= InventoryUI.Instance.inventory.UpdateItem;
    }
}
