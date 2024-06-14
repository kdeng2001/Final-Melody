using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsList : MonoBehaviour
{
    public static ItemsList Instance;
    public Dictionary<string, Item> items;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Item[] itemComponents = GetComponentsInChildren<Item>();
            items = new();
            foreach(Item i in itemComponents)
            {
                items[i.itemName] = i;
                i.gameObject.SetActive(false);
            }
        }
    }

    public void UseItem(string itemName)
    {
        items[itemName].UseItem();
    }
}
