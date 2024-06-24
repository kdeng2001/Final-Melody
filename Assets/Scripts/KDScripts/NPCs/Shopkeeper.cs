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
        }
        // if shop keeper finishes thanking player
        else if(currIndex == 1)
        {
            currIndex = 0;
            Time.timeScale = 1;
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
    public Item item;
    public int cost;
    public int amount;
}
