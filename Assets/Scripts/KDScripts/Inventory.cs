using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public int money = 0;
    [SerializeField] public int reputation = 0;
    public Dictionary<string, int> items;
    public void AddMoney(int amount) { money += amount; }
    public void RemoveMoney(int amount) { money -= amount; }
    public void AddReputation(int amount) { reputation += amount; }
    public void RemoveReputation(int amount) { reputation -= amount; }
    private void Awake()
    {
        items = new Dictionary<string, int>();
    }
    public void AddItems(string name, int count) 
    {
        if(count <= 0) { return; }
        if(items.ContainsKey(name)) { items[name] += count; }
        else { items[name] = count; }
        Debug.Log("You have " + items[name] + " of " + name);

    }
    public void RemoveItems(string name, int count) 
    {
        if(count >= 0) { return; }
        if(items.ContainsKey(name)) 
        {
            if(count >= items[name]) { items.Remove(name); }
            else { items[name] -= count; }
        }
    }
}
