using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int sceneIndex;
    public Vector3 playerPosition;
    public SerializableDictionary<string, string> itemAmountInventory;
    public SerializableDictionary<string, string> itemIconInventory;
    public SerializableDictionary<string, bool> itemInScene;
    public GameData()
    {
        playerPosition = Vector3.zero;
        itemAmountInventory = new();
        itemIconInventory = new();
        itemInScene = new();
    }
}
