using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    [SerializeField] public string itemName = "item";
    [SerializeField] public string id;
    [SerializeField] public int amount = 1;
    //[System.NonSerialized]
    public bool obtained = false;

    public void LoadData(GameData data)
    {
        if(data.itemInScene.TryGetValue(id, out bool collected))
        {
            obtained = collected;
            gameObject.SetActive(!collected);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.itemInScene.ContainsKey(id)) { data.itemInScene.Remove(id); }
        data.itemInScene[id] = obtained;
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
