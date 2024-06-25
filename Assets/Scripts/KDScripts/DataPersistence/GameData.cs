using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // scene
    public int sceneIndex;
    public SerializableDictionary<string, bool> itemInScene;

    // player
    public Vector3 playerPosition;
    
    // inventory
    public int money;
    public int reputation;
    public SerializableDictionary<string, string> itemAmountInventory;
    public SerializableDictionary<string, string> itemIconInventory;
    public SerializableDictionary<string, string> inkVariables;
    public SerializableDictionary<string, string> shopkeepers;

    public GameData()
    {
        sceneIndex = 1;
        playerPosition = Vector3.up;
        itemAmountInventory = new();
        itemIconInventory = new();
        itemInScene = new();
        shopkeepers = new();
    }
}
