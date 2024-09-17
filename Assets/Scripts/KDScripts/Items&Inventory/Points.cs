using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour, IDataPersistence
{
    [SerializeField] public int money { get; private set; }
    [SerializeField] public int reputation { get; private set; }

    public Points(int money, int reputation)
    {
        this.money = money;
        this.reputation = reputation;
    }

    public void UpdateMoney(int amount)
    {
        // compute new sum
        int sum = money + amount;
        // fail transaction if new sum is negative
        if (sum < 0) { return; }
        money += amount;
        InventoryUI.Instance.UpdateMoneyUI(money);
    }

    public void UpdateReputation(int amount)
    {
        reputation += amount;
        InventoryUI.Instance.UpdateReputationUI(reputation);
    }

    public void LoadData(GameData data)
    {
        money = 0;
        reputation = 0;
        UpdateMoney(data.money);
        UpdateReputation(data.reputation);
    }

    public void SaveData(/*ref */GameData data)
    {
        data.money = money;
        data.reputation = reputation;
    }
}
