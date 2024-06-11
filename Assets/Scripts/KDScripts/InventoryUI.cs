using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemEntry;
    public Dictionary<string, GameObject> itemEntries;
    public Inventory inventory { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
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

    public void Clear()
    {
        foreach(GameObject uiEntry in itemEntries.Values)
        {
            Destroy(uiEntry);
        }
        itemEntries.Clear();
    }

    public void UpdateItemEntry(string itemName, int amount)
    {
        // find if item entry already exists
        // if so, update the ui entry

        if (itemEntries.ContainsKey(itemName))
        {
            Debug.Log(itemName + " exists");
            UpdateUIText(itemName, amount);
        }
        //else, create a new entry
        else
        {
            Debug.Log(itemName + " does not already exist");
            GameObject newEntry = Instantiate(itemEntry);
            newEntry.transform.SetParent(content, false);
            newEntry.SetActive(true);
            itemEntries[itemName] = newEntry;
            UpdateUIText(itemName, amount);
        }
    }
    private void UpdateUIText(string itemName, int amount)
    {
        List<TextMeshProUGUI> tmps = itemEntries[itemName].GetComponentsInChildren<TextMeshProUGUI>().ToList();
        foreach (TextMeshProUGUI tmp in tmps)
        {
            if (tmp.name == "Name") { tmp.text = itemName; }
            else if (tmp.name == "Count")
            {
                // compute new item amount
                Debug.Log("amount: " + amount);
                int newAmount = int.Parse(tmp.text.Substring(1)) + amount;
                // remove if newAmount less than 0
                if (newAmount <= 0)
                {
                    Debug.Log("Destroy " + itemName);
                    Destroy(itemEntries[itemName]);
                    itemEntries.Remove(itemName);
                    break;
                }
                // add in 0 to front
                else if (newAmount < 10) { tmp.text = "0" + newAmount.ToString(); }
                else { tmp.text = newAmount.ToString(); }
                // add x in front
                tmp.text = "x" + tmp.text;

            }
        }
    }
}
