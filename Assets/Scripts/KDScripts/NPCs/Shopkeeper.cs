using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shopkeeper : NPCInteractable, IDataPersistence
{
    // player initiates dialogue
    // shop keeper welcomes player (Dialogue 1)
    // shop menu pops up
    // on shop menu exit -> shop keeper thanks player (Dialogue 2) 

    [Header("Shopkeeper goods")]
    [SerializeField] private List<ShopItem> itemsForSale;
    [Header("ID for data persistence across scenes and sessions")]
    public string id;
    public override void OnFinishInteract()
    {
        isTalking = false;
        // if shopkeeper finishes welcoming player
        if(currIndex == 0)
        {

            currIndex = 1;
            // display shop menu
            ShopUIManager.Instance.DisplayShopMenu(this);
            // prevent player from talking to shop keeper while shopping menu is displayed
            Player.Instance.PauseInteractor();
            Player.Instance.PauseMovement();
            enabled = false;
            // listen for finish shopping to start thanking player
            ShopUIManager.Instance.finishShopping += OnStartInteract;
        }
        // if shop keeper finishes thanking player
        else if(currIndex == 1)
        {
            currIndex = 0;
            // stop listening for finish shopping once shopping is done
            ShopUIManager.Instance.finishShopping -= OnStartInteract;
            Time.timeScale = 1;
            interacting = false;
        }
    }

    public override void OnStartInteract()
    {
        // prepare the shopping experience
        if(currIndex == 0) { ShopUIManager.Instance.PopulateShopMenu(itemsForSale); interacting = true; }
        // allow player to finish shopping
        if (currIndex == 1) 
        {
            Player.Instance.UnpauseInteractor();
            enabled = true;
        }
        base.OnStartInteract();
    }

    public void UpdateItemsForSale(string itemName, int itemAmount)
    {
        for (int i = 0; i < itemsForSale.Count; i++)
        {
            if(itemsForSale[i].itemName == itemName)
            {
                ShopItem temp = itemsForSale[i];
                temp.stockOfUnits += itemAmount;
                itemsForSale[i] = temp;
                break;
            }
        }
    }
    [ContextMenu("Generate guid for id")]
    private void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        // decode itemForSale data, where even strings are itemName, odd strings are stockOfUnits
        // set the string values to the corresponding itemForSale
        string decode;
        if(!data.shopkeepers.TryGetValue(id, out decode)) { return; }
        int index = 0;
        Dictionary<string, int> itemsTemp = new();
        // parse data
        while(index < decode.Length)
        {
            int strLength = 0;
            int count = 0;
            // get length of itemName
            while(decode[index] != '#')
            {
                count++;
                index++;
            }
            // get itemName
            strLength = int.Parse(decode.Substring(index - count, count));
            string itemName = decode.Substring(index + 1, strLength);
            index += strLength + 1;

            strLength = 0;
            count = 0;
            // get length of stockOfUnits
            while (decode[index] != '#')
            {
                count++;
                index++;
            }
            // get stockOfUnits
            strLength = int.Parse(decode.Substring(index - count, count));
            int itemCount = int.Parse(decode.Substring(index + 1, strLength));

            itemsTemp[itemName] = itemCount;
            index+= strLength + 1;
        }
        // load data
        for(int i=0; i<itemsTemp.Count; i++) 
        {
            ShopItem temp = itemsForSale[i];
            temp.stockOfUnits = itemsTemp[itemsForSale[i].itemName];
            itemsForSale[i] = temp;
        }
    }

    public void SaveData(GameData data)
    {
        // clear data if exists
        if(data.shopkeepers.ContainsKey(id)) { data.shopkeepers.Remove(id); }
        // encode itemForSale itemName and stockOfUnits into a single string
        string encode = "";
        foreach(ShopItem item in itemsForSale)
        {
            encode += string.Concat(
                item.itemName.Length.ToString(), "#", item.itemName, 
                item.stockOfUnits.ToString().Length, "#", item.stockOfUnits.ToString() 
                );
        }
        data.shopkeepers[id] = encode;
    }
}

[System.Serializable]
public struct ShopItem
{
    public Sprite icon;
    public string itemName;
    public int costPerUnit;
    public int stockOfUnits;
}
