using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRemoveItem : MonoBehaviour
{
    [SerializeField] int removeAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) { return; }
        Debug.Log("test remove item");
        foreach(string key in InventoryUI.Instance.itemEntries.Keys)
        {
            InventoryUI.Instance.RemoveItemTest(key, removeAmount);
            break;
        }
    }
}
