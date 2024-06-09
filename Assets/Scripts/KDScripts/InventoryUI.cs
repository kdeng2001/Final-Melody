using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour
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

    private void UpdateItemCountUI(TextMeshProUGUI tmp, Item item)
    {
        tmp.text = (int.Parse(tmp.text.Substring(1)) + item.amount).ToString();
        if (tmp.text.Length <= 1) { tmp.text = "0" + tmp.text; }
        tmp.text = "x" + tmp.text;
        inventory.AddItems(item.itemName, item.amount);
    }
}
