using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistence
{
    [SerializeField] public int money = 0;
    [SerializeField] public int reputation = 0;
    public Dictionary<string, int> items;
    public void AddMoney(int amount) { money += amount; }
    public void RemoveMoney(int amount) { money -= amount; }
    public void AddReputation(int amount) { reputation += amount; }
    public void RemoveReputation(int amount) { reputation -= amount; }
    private void Awake()
    {
        items = new Dictionary<string, int>();
    }
    public void UpdateItem(string itemName, int amount) 
    {
        // if itemName exists in items
        if(items.ContainsKey(itemName)) 
        { 
            // add amount to existing amount
            items[itemName] += amount; 
            // if amount less than 0, remove from items, otherwise keep
            if(items[itemName] <= 0) { items.Remove(itemName); }
        }
        // else if itemName does not exist and amount is positive, set amount directly
        else if(amount > 0){ items[itemName] = amount; }

        if (items.ContainsKey(itemName)) { Debug.Log("You have " + items[itemName] + " of " + itemName); }
        InventoryUI.Instance.UpdateItemEntry(itemName, amount);

    }

    public void LoadData(GameData data)
    {
        Debug.Log("Load inventory");
        items.Clear();
        InventoryUI.Instance.Clear();
        List<string> keys = data.itemInventory.Keys.ToList();
        foreach (string key in keys)
        {
            UpdateItem(key, data.itemInventory[key]);
        }
    }

    public void SaveData(ref GameData data)
    {        
        Debug.Log("Save inventory");
        data.itemInventory.Clear();
        List<string> keys = items.Keys.ToList();
        foreach(string key in keys)
        {
            data.itemInventory[key] = items[key];
        }
    }

    //public void RemoveItem(string itemName, int amount) 
    //{
    //    if(amount >= 0) { return; }
    //    if(items.ContainsKey(itemName)) 
    //    {
    //        if(amount >= items[itemName]) { items.Remove(itemName); }
    //        else { items[itemName] -= amount; }
    //    }
    //    InventoryUI.Instance.RemoveItemEntry(itemName, amount);
    //}
}
