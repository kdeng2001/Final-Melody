using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] Item item;
    public delegate void ItemWasObtained(Item item);
    public ItemWasObtained itemWasObtained;
    public override void OnFinishInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStartInteract()
    {
        Debug.Log("start interacting with " + transform.parent.name);
        itemWasObtained?.Invoke(item);
        transform.parent.gameObject.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
        itemWasObtained += InventoryUI.Instance.AddItemEntry;
    }
    private void OnDestroy()
    {
        itemWasObtained -= InventoryUI.Instance.AddItemEntry;
    }
}