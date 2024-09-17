using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemEntry;
    public Dictionary<string, GameObject> itemEntries;
    [SerializeField] public Inventory inventory;
    [SerializeField] public Points points;
    [SerializeField] private TextMeshProUGUI moneyAmount;
    [SerializeField] private TextMeshProUGUI reputationAmount;

    [Header("Use Item Confirmation Pop up")]
    [SerializeField] private RectTransform confirmationPopup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            itemEntries = new Dictionary<string, GameObject>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Clear()
    {
        foreach (GameObject uiEntry in itemEntries.Values)
        {
            Destroy(uiEntry);
        }
        itemEntries.Clear();
    }

    public void UpdateItemEntry(string itemName, int amount, string iconPath)
    {
        // find if item entry already exists
        // if so, update the ui entry
        if (itemEntries.ContainsKey(itemName))
        {
            UpdateUI(itemName, amount);
        }
        //else, create a new entry
        else
        {
            GameObject newEntry = Instantiate(itemEntry);
            newEntry.transform.SetParent(content, false);
            newEntry.SetActive(true);
            itemEntries[itemName] = newEntry;
            UpdateUI(itemName, amount);
            CreateIcon(itemName, iconPath);
            Button button = itemEntries[itemName].GetComponent<Button>();
            button.onClick.AddListener(() => HandleConfirmationPopup(itemName));
        }
    }
    private void UpdateUI(string itemName, int amount)
    {
        Debug.Log("updateUI");
        List<TextMeshProUGUI> tmps = itemEntries[itemName].GetComponentsInChildren<TextMeshProUGUI>().ToList();
        foreach (TextMeshProUGUI tmp in tmps)
        {
            if (tmp.name == "Name") { tmp.text = itemName; }
            else if (tmp.name == "Count")
            {
                // compute new item amount
                int newAmount = int.Parse(tmp.text.Substring(1)) + amount;
                // remove if newAmount less than 0
                if (newAmount <= 0)
                {
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
    /// <summary>
    /// assumes that the icon is the only image, aside from the background image in the parent button
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="iconPath"></param>
    private void CreateIcon(string itemName, string iconPath)
    {
        Image img = itemEntries[itemName].GetComponentsInChildren<Image>()[1];
        img.sprite = Resources.Load<Sprite>(iconPath);
    }

    public void UpdateMoneyUI(int newValue)
    {
        Debug.Log("updateUI");
        moneyAmount.text = newValue.ToString();
    }
    public void UpdateReputationUI(int newValue)
    {
        Debug.Log("updateUI");
        reputationAmount.text = newValue.ToString();
    }

    public void HandleConfirmationPopup(string itemName)
    {
        // get confirmation text
        // change text to match relevant itemName
        foreach(TextMeshProUGUI tmp in confirmationPopup.gameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if(tmp.name == "ConfirmationText") 
            {
                tmp.text = "Use " + itemName + "?";
                break;
            }
        }
        // get confirmation 'Yes' button
        // set onClick method to use item
        foreach(Button b in confirmationPopup.gameObject.GetComponentsInChildren<Button>())
        {
            if(b.name == "Yes")
            {
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(
                    () =>
                    {
                        ItemsList.Instance.UseItem(itemName);
                        confirmationPopup.gameObject.SetActive(false);
                    }
                );
                break;
            }
        }
        // make confirmationPopup active
        confirmationPopup.gameObject.SetActive(true);
    }
}
