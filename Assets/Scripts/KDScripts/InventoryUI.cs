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
    }

    private void UpdateItemCountUI(TextMeshProUGUI tmp, Item item)
    {
        tmp.text = (int.Parse(tmp.text.Substring(1)) + item.amount).ToString();
        if (tmp.text.Length <= 1) { tmp.text = "0" + tmp.text; }
        tmp.text = "x" + tmp.text;
        inventory.AddItems(item.itemName, item.amount);
    }
    private void UpdateItemCountUI(TextMeshProUGUI tmp, string itemName, int count)
    {
        tmp.text = (int.Parse(tmp.text.Substring(1)) + count).ToString();
        if (tmp.text.Length <= 1) { tmp.text = "0" + tmp.text; }
        tmp.text = "x" + tmp.text;
        inventory.AddItems(itemName, count);
    }

    public void LoadData(GameData data)
    {
        // make sure inventory is empty
        inventory.items.Clear();
        itemEntries.Clear();
        List<RectTransform> uiEntries = content.gameObject.GetComponentsInChildren<RectTransform>().ToList();
        uiEntries.RemoveAt(0);
        foreach(RectTransform ui in uiEntries) { Destroy(ui.gameObject); }
        // add back each item and its amount from data
        foreach (KeyValuePair<string, int> pair in data.itemInventory)
        {
            AddItemEntry(pair.Key, pair.Value);
        }
    }

    public void SaveData(ref GameData data)
    {
        // make sure data for inventory is empty
        //data.itemInventory.Clear();

        // add back in each item and its amount to data
        foreach (KeyValuePair<string, int> pair in inventory.items)
        {
            data.itemInventory[pair.Key] = pair.Value;
        }

    }
}
