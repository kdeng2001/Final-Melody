using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingEntry : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI stockAmount;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI buyAmount;
    public HoldClickableButton incrementBuyAmount;
    public HoldClickableButton decrementBuyAmount;
    public RectTransform soldOutFilter;
    public Button buyButton;
    public Image icon;
    public int stockAmountVal;
    public int costVal;
    public int buyAmountVal;
}
