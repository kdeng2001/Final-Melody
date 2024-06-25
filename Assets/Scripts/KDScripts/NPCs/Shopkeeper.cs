using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : NPCInteractable
{
    // player initiates dialogue
    // shop keeper welcomes player (Dialogue 1)
    // shop menu pops up
    // on shop menu exit -> shop keeper thanks player (Dialogue 2) 
    //private int shopkeeperIndex = 0;

    [Header("Shopkeeper goods")]
    [SerializeField] private List<ShopItem> itemsForSale;
    public bool isShopping { get; private set; }
    private void Awake()
    {
        isShopping = false;
    }
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
        }
    }

    public override void OnStartInteract()
    {
        // prepare the shopping experience
        if(currIndex == 0) { ShopUIManager.Instance.PopulateShopMenu(itemsForSale);}
        // allow player to finish shopping
        if (currIndex == 1) 
        {
            //Debug.Log("Start interact index = " + currIndex);
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

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
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
