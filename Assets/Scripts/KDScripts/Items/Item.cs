using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Item : MonoBehaviour, IDataPersistence
{
    [SerializeField] public string itemName = "item";
    [SerializeField] public string id;
    [SerializeField] public int amount = 1;
    [SerializeField] public bool reusable = true;
    public string iconFilePath { get; private set; }

    [SerializeField] private GameObject model;
    [SerializeField] private GameObject interactable;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private SpriteRenderer icon;
    //[System.NonSerialized]
    public bool obtained = false;

    private void Awake()
    {
        iconFilePath = AssetDatabase.GetAssetPath(icon.sprite);
        //Debug.Log("iconFilePath: " + iconFilePath);
    }

    public abstract void UseItem();

    public void LoadData(GameData data)
    {
        if(data.itemInScene.TryGetValue(id, out bool obtained))
        {
            this.obtained = obtained;
            //Debug.Log(itemName + " was in data and obtained: " + obtained);
            if (this == null) { return; }
            //Debug.Log(itemName + ", " + gameObject.name + " was in data and obtained: " + obtained);
            if (obtained) { Destroy(gameObject); }
            gameObject.SetActive(true);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.itemInScene.ContainsKey(id)) { data.itemInScene.Remove(id); }
        //Debug.Log(itemName + " is saving... " + obtained);
        data.itemInScene[id] = obtained;
        if(this == null) { return; }
        //Debug.Log(itemName + ", " + gameObject.name + " was obtained: " + obtained);
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public void HandleObtained()
    {
        model.SetActive(false);
        interactable.SetActive(false);
        interactIcon.SetActive(false);
        obtained = true;
    }
}
