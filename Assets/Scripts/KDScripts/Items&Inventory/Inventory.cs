using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, IDataPersistence
{
    /// <summary>
    /// This dictionary stores a key: name, and string[], containing the amount at string[0] and iconFilePath at string[1]
    /// </summary>
    public Dictionary<string, string> itemAmounts;
    public Dictionary<string, string> itemIcons;
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
            InventoryUI.Instance.UpdateItemEntry(itemName, amount, itemIcons[itemName]);            
            if (newAmount <= 0) 
                { 
                    itemAmounts.Remove(itemName);
                    itemIcons.Remove(itemName);
             }
        }
    }

    public void LoadData(GameData data)
    {
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

    public void SaveData(GameData data)
    {        
        data.itemAmountInventory.Clear();
        data.itemIconInventory.Clear();
        List<string> keys = itemAmounts.Keys.ToList();
        foreach(string key in keys)
        {
            data.itemAmountInventory[key] = itemAmounts[key] ;
            data.itemIconInventory[key] = itemIcons[key];
        }
    }
}
