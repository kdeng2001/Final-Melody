using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour, IDataPersistence
{
    public static InventoryUI Instance { get; private set; }
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemEntry;
    public Dictionary<string, GameObject> itemEntries;
    private Inventory inventory;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.Log("Destroy InventoryUI");
            Destroy(gameObject);
            return;
        }
        else 
        {
            Instance = this;
            itemEntries = new Dictionary<string, GameObject>();
            inventory = gameObject.AddComponent<Inventory>();
            DontDestroyOnLoad(gameObject);
        }
    }
    public void AddItemEntry(Item item)
    {
        // if item exists in inventory, update the amount
        item.obtained = true;
        if(itemEntries.ContainsKey(item.itemName))
        {
            var UGUIs = itemEntries[item.itemName].GetComponentsInChildren<TextMeshProUGUI>();
            foreach(TextMeshProUGUI tmp in UGUIs)
            {
                if(tmp.name == "Count") { UpdateItemCountUI(tmp, item); }
            }
        }
        // item does not exist in inventory, add an entry with name and amount for item
        else
        {
            GameObject entry = Instantiate(itemEntry);
            entry.transform.SetParent(content, false);
            entry.SetActive(true);
            itemEntries[item.itemName] = entry;
            var UGUIs = itemEntries[item.itemName].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tmp in UGUIs)
            {
                //if (tmp.name == "Count") { Debug.Log(tmp.text + " "); }
                if (tmp.name == "Count") { UpdateItemCountUI(tmp, item); }
                else if (tmp.name == "Name") { tmp.text = item.itemName; }
            }
        }
        inventory.AddItems(item.itemName, item.amount);
    }
    public void AddItemEntry(string itemName, int count)
    {
        if(itemEntries.ContainsKey(itemName))
        {
            var UGUIs = itemEntries[itemName].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tmp in UGUIs)
            {
                if (tmp.name == "Count") { UpdateItemCountUI(tmp, itemName, count); }
            }
        }
        else
        {
            GameObject entry = Instantiate(itemEntry);
            entry.transform.SetParent(content, false);
            entry.SetActive(true);
            itemEntries[itemName] = entry;
            var UGUIs = itemEntries[itemName].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tmp in UGUIs)
            {
                //if (tmp.name == "Count") { Debug.Log(tmp.text + " "); }
                if (tmp.name == "Count") { UpdateItemCountUI(tmp, itemName, count); }
                else if (tmp.name == "Name") { tmp.text = itemName; }
            }
        }
        inventory.AddItems(itemName, count);
    }

    private void UpdateItemCountUI(TextMeshProUGUI tmp, Item item)
    {
        tmp.text = (int.Parse(tmp.text.Substring(1)) + item.amount).ToString();
        if (tmp.text.Length <= 1) { tmp.text = "0" + tmp.text; }
        tmp.text = "x" + tmp.text;
    }
    private void UpdateItemCountUI(TextMeshProUGUI tmp, string itemName, int count)
    {
        tmp.text = (int.Parse(tmp.text.Substring(1)) + count).ToString();
        if (tmp.text.Length <= 1) { tmp.text = "0" + tmp.text; }
        tmp.text = "x" + tmp.text;
    }
    public void RemoveItemTest(string itemName, int amount)
    {
        if(itemEntries.ContainsKey(itemName) && inventory.items.ContainsKey(itemName))
        {
            inventory.items[itemName] -= amount;
            if(inventory.items[itemName] <= 0)
            {
                inventory.items.Remove(itemName);
                Destroy(itemEntries[itemName]);
                itemEntries.Remove(itemName);
                Debug.Log(itemName + " was removed");
            }
            else
            {
                foreach(TextMeshProUGUI t in itemEntries[itemName].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    if(t.name == "Count") { UpdateItemCountUI(t, itemName, -amount); }
                }
            }
            
        }
    }
    public void LoadData(GameData data)
    {
        if(this == null || gameObject == null || inventory == null || itemEntries == null) { return; }
        // make sure inventory is empty
        inventory.items.Clear();
        itemEntries.Clear();
        // clear out UI
        List<RectTransform> uiEntries = content.gameObject.GetComponentsInChildren<RectTransform>().ToList();
        uiEntries.RemoveAt(0);
        foreach(RectTransform ui in uiEntries) { Destroy(ui.gameObject); }
        
        // add back each item and its amount from data to inventory + ui
        foreach (KeyValuePair<string, int> pair in data.itemInventory)
        {
            AddItemEntry(pair.Key, pair.Value);
        }
        Debug.Log("Finish loading inventory UI data");
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("saving...");
        if(this == null || gameObject == null) { return; }
        
        // make sure data for inventory is empty
        data.itemInventory.Clear();

        // add back in each item and its amount to data
        foreach (KeyValuePair<string, int> pair in inventory.items)
        {
            data.itemInventory[pair.Key] = pair.Value;
        }
        Debug.Log("Finish saving inventory UI data");

    }
}
