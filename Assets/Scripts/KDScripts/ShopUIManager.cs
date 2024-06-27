using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [Header("Shop Menu Animator")]
    [SerializeField] private Animator shopMenuAnimator;
    [Header("Buy Item Entry UI Template")]
    [SerializeField] private GameObject BuyItemEntry;
    [Header("Shop Menu UI")]
    [SerializeField] private RectTransform shopMenu;
    [SerializeField] private RectTransform shopContent;
    [Header("Shop Exit Button")]
    [SerializeField] private Button ExitButton;
    [Header("Player Cash")]
    [SerializeField] private TextMeshProUGUI cash;

    private Dictionary<string, ShoppingEntry> itemsUI;
    public static ShopUIManager Instance;
    // reference to shopkeeper currently trading
    public Shopkeeper currentShopkeeper { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            currentShopkeeper = null;
            //shopMenu.gameObject.SetActive(false);
            //DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        itemsUI = new Dictionary<string, ShoppingEntry>();
    }

    // GENERAL SHOP FUNCTIONS
    public void PopulateShopMenu(List<ShopItem> shopItems) 
    {
        cash.text = InventoryUI.Instance.points.money.ToString();
        ClearShopMenu();
        itemsUI.Clear();
        int index = 0;
        foreach(ShopItem item in shopItems)
        {
            (itemsUI[item.itemName]) = CreateShoppingEntry(item);
            itemsUI[item.itemName].transform.SetParent(shopContent, false);
            RectTransform itemT = itemsUI[item.itemName].transform.GetComponent<RectTransform>();
            itemT.localPosition = new(-550, itemT.localPosition.y - index * itemT.rect.height, 0);
            itemsUI[item.itemName].gameObject.SetActive(true);
            index++;
        }
    }

    public void ClearShopMenu()
    {
        foreach(ShoppingEntry shopItem in itemsUI.Values)
        {
            Destroy(shopItem.gameObject);
        }
    }

    private ShoppingEntry CreateShoppingEntry(ShopItem item)
    {
        GameObject entry = Instantiate(BuyItemEntry);
        ShoppingEntry entryData = entry.GetComponent<ShoppingEntry>();
        // set icon image
        entryData.icon.sprite = item.icon;

        // set item name
        entryData.itemName.text = item.itemName;

        // set item cost
        entryData.costVal = item.costPerUnit;
        entryData.cost.text = "$" + entryData.costVal.ToString();

        // set buy amount
        entryData.buyAmount.text = "x <b>00</b>";
        entryData.buyAmountVal = 0;

        // set stock amount
        entryData.stockAmountVal = item.stockOfUnits;
        entryData.stockAmount.text = entryData.stockAmountVal.ToString();
        if(entryData.stockAmountVal == 0) { entryData.soldOutFilter.gameObject.SetActive(true); }

        // listen to increment/decrement events
        entryData.incrementBuyAmount.OnClicked += () => IncrementBuyAmount(item.itemName);
        entryData.decrementBuyAmount.OnClicked += () => DecrementBuyAmount(item.itemName);
        entryData.incrementBuyAmount.OnHoldClicked += () => IncrementBuyAmount(item.itemName);
        entryData.decrementBuyAmount.OnHoldClicked += () => DecrementBuyAmount(item.itemName);

        entryData.buyButton.onClick.AddListener(() => Buy(item.itemName));
        return entryData;
    }

    public delegate void StartShopping();
    public StartShopping startShopping;
    public void DisplayShopMenu(Shopkeeper shopkeeper) 
    {
        startShopping?.Invoke();
        // disable opening menu
        InGameMenu.Instance.enabled = false;
        currentShopkeeper = shopkeeper;
        shopMenu.gameObject.SetActive(true);
        shopMenuAnimator.Play("ShopPopup");
    }
    public delegate void FinishShopping();
    public FinishShopping finishShopping;
    public void HideShopMenu() 
    {
        shopMenuAnimator.Play("ShopUnpop");
        //shopMenu.gameObject.SetActive(false);
        // enable opening menu
        InGameMenu.Instance.enabled = true;
        currentShopkeeper = null;
        finishShopping?.Invoke();
    }
    // SHOP ENTRY FUNCTIONS
    public void Buy(string itemName) 
    {
        // if amount more than stock, return
        if(itemsUI[itemName].buyAmountVal > itemsUI[itemName].stockAmountVal) { return; }
        // if amount is 0
        if(itemsUI[itemName].buyAmountVal == 0) { return; }

        // if not purchasable, return
        int cost = (itemsUI[itemName].buyAmountVal + 1) * itemsUI[itemName].costVal;
        if(cost > InventoryUI.Instance.points.money) { return; }
        // if purchasable
        // add to player inventory
        InventoryUI.Instance.inventory.UpdateItem(itemName, itemsUI[itemName].buyAmountVal, ItemsList.Instance.items[itemName].iconFilePath);
        // decrement player money
        // update player money ui
        InventoryUI.Instance.points.UpdateMoney(-cost);
        cash.text = InventoryUI.Instance.points.money.ToString();

        // decrease stock inventory
        itemsUI[itemName].stockAmountVal -= itemsUI[itemName].buyAmountVal;
        // update shopkeeper stock
        currentShopkeeper.UpdateItemsForSale(itemName, -itemsUI[itemName].buyAmountVal);
        // update stock ui
        itemsUI[itemName].stockAmount.text = itemsUI[itemName].stockAmountVal.ToString();
        if(itemsUI[itemName].stockAmountVal == 0) { itemsUI[itemName].soldOutFilter.gameObject.SetActive(true); }
    }
    public void IncrementBuyAmount(string itemName)
    {

        // do not increment if cost greater than money
        int cost = (itemsUI[itemName].buyAmountVal + 1) * itemsUI[itemName].costVal;
        if(InventoryUI.Instance.points.money <  cost) { return; }
        // otherwise, increment
        Debug.Log("incrementing...");
        if (itemsUI.ContainsKey(itemName) && itemsUI[itemName].buyAmountVal < itemsUI[itemName].stockAmountVal) 
        {
            itemsUI[itemName].buyAmountVal += 1;
            itemsUI[itemName].buyAmount.text = itemsUI[itemName].buyAmountVal > 9 ? 
                "x <b>" + itemsUI[itemName].buyAmountVal.ToString() + "</b>" :
                "x <b>0" + itemsUI[itemName].buyAmountVal.ToString() + "</b>";
            itemsUI[itemName] = itemsUI[itemName];
        }
    }
    public void DecrementBuyAmount(string itemName)
    {
        Debug.Log("decrementing...");
        if (itemsUI.ContainsKey(itemName) && itemsUI[itemName].buyAmountVal > 0)
        {
            itemsUI[itemName].buyAmountVal -= 1;
            itemsUI[itemName].buyAmount.text = itemsUI[itemName].buyAmountVal > 9 ?
                "x <b>" + itemsUI[itemName].buyAmountVal.ToString() + "</b>" :
                "x <b>0" + itemsUI[itemName].buyAmountVal.ToString() + "</b>";
            //ShoppingEntry temp = items[itemName];
            //temp.buyAmountVal -= 1;
            //temp.buyAmount.text = temp.buyAmountVal > 9 ?
            //    "x <b>" + temp.buyAmountVal.ToString() + "</b>" :
            //    "x <b>0" + temp.buyAmountVal.ToString() + "</b>";
            //items[itemName] = temp;
        }
    }
    //IEnumerator HoldDecrement()
    //{
    //    while(ExitButton.on) { }
    //}


}

//public class ShoppingEntry
//{
//    public TextMeshProUGUI itemName;
//    public TextMeshProUGUI stockAmount;
//    public TextMeshProUGUI cost;
//    public TextMeshProUGUI buyAmount;
//    public HoldClickableButton incrementBuyAmount;
//    public HoldClickableButton decrementBuyAmount;

//    public int stockAmountVal;
//    public int costVal;
//    public int buyAmountVal;


//}

