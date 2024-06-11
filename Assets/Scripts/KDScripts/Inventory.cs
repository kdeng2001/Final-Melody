using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistence
{
    [SerializeField] public int money = 0;
    [SerializeField] public int reputation = 0;
    /// <summary>
    /// This dictionary stores a key: name, and string[], containing the amount at string[0] and iconFilePath at string[1]
    /// </summary>
    public Dictionary<string, string> itemAmounts;
    public Dictionary<string, string> itemIcons;
    public void AddMoney(int amount) { money += amount; }
    public void RemoveMoney(int amount) { money -= amount; }
    public void AddReputation(int amount) { reputation += amount; }
    public void RemoveReputation(int amount) { reputation -= amount; }
    private void Awake()
    {
        itemAmounts = new Dictionary<string, string>();
        itemIcons = new Dictionary<string, string>();
    }
    public void UpdateItem(string itemName, int amount, string iconPath) 
    {
        // if itemName exists in items
        if(itemAmounts.ContainsKey(itemName)) 
        {
            // add amount to existing amount
            int newAmount = int.Parse(itemAmounts[itemName]) + amount;
            itemAmounts[itemName] = newAmount.ToString();
            itemIcons[itemName] = iconPath;
            // if amount less than 0, remove from items, otherwise keep
            if(newAmount <= 0) 
            { 
                itemAmounts.Remove(itemName);
                itemIcons.Remove(itemName);
            }
        }
        // else if itemName does not exist and amount is positive, set amount directly, and set iconPath
        else if(amount > 0)
        { 
            itemAmounts[itemName] = amount.ToString();
            itemIcons[itemName] = iconPath;
        }

        if (itemAmounts.ContainsKey(itemName)) { Debug.Log("You have " + itemAmounts[itemName] + " of " + itemName); }
        InventoryUI.Instance.UpdateItemEntry(itemName, amount, iconPath);

    }
    /// <summary>
    /// for existing items that already exist in inventory only
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    public void UpdateItem(string itemName, int amount)
    {
        // if itemName exists in items
        if (itemAmounts.ContainsKey(itemName))
        {
            // add amount to existing amount
            int newAmount = int.Parse(itemAmounts[itemName]) + amount;
            itemAmounts[itemName] = newAmount.ToString();
            // if amount less than 0, remove from items, otherwise keep
            if (itemAmounts.ContainsKey(itemName)) { Debug.Log("You have " + itemAmounts[itemName] + " of " + itemName); }
            InventoryUI.Instance.UpdateItemEntry(itemName, amount, itemIcons[itemName]);            
            if (newAmount <= 0) 
                { 
                    itemAmounts.Remove(itemName);
                    itemIcons.Remove(itemName);
             }
        }
        // else if itemName does not exist and amount is positive, set amount directly, and set iconPath
   

    }

    public void LoadData(GameData data)
    {
        Debug.Log("Load inventory");
        itemAmounts.Clear();
        itemIcons.Clear();
        InventoryUI.Instance.Clear();
        List<string> keys = data.itemAmountInventory.Keys.ToList();
        foreach (string key in keys)
        {
            // update with key (name), amount, and icon filePath
            UpdateItem(key, int.Parse(data.itemAmountInventory[key]), data.itemIconInventory[key]);
        }
    }

    public void SaveData(ref GameData data)
    {        
        Debug.Log("Save inventory");
        data.itemAmountInventory.Clear();
        data.itemIconInventory.Clear();
        List<string> keys = itemAmounts.Keys.ToList();
        foreach(string key in keys)
        {
            
            data.itemAmountInventory[key] = itemAmounts[key] ;
            data.itemIconInventory[key] = itemIcons[key];
            //Debug.Log("adding key: " + key + " and values: " + data.itemInventory[key][0] + ", " + data.itemInventory[key][1]);
            //data.itemInventory[key] = items[key];
        }
    }
}
