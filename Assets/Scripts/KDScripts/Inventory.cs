using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public int money = 0;
    [SerializeField] public int reputation = 0;
    public Dictionary<string, int> items { get; private set; }
    public void AddMoney(int amount) { money += amount; }
    public void RemoveMoney(int amount) { money -= amount; }
    public void AddReputation(int amount) { reputation += amount; }
    public void RemoveReputation(int amount) { reputation -= amount; }
    public void AddItems(string name, int count) 
    {
        if(count <= 0) { return; }
        if(items.ContainsKey(name)) { items[name] += count; }
        else { items[name] = count; }
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
