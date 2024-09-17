using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Item : MonoBehaviour, IDataPersistence
{
    [Header("Audio")]
    [SerializeField] public AK.Wwise.Event pickupWwiseSFX;
    [SerializeField] public string itemName = "item";
    [SerializeField] public string id;
    [SerializeField] public int amount = 1;
    [SerializeField] public bool reusable = true;
    public string iconFilePath { get; private set; }
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject interactable;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private SpriteRenderer icon;

    public bool obtained = false;
    public bool isSoundPlaying { get; private set; }
    private void Awake()
    {
        iconFilePath = icon.sprite.name;
        isSoundPlaying = false;
    }

    public abstract void UseItem();
    public void PlaySFX()
    {
        if(pickupWwiseSFX != null && !isSoundPlaying) 
        {
            if (AudioManager.Instance.itemAudio != null) { AudioManager.Instance.itemAudio.StopSFX(false); }
            pickupWwiseSFX.Post(AudioManager.Instance.gameObject, (uint)AkCallbackType.AK_EndOfEvent, CallBackFunction);
            isSoundPlaying = true;
            AudioManager.Instance.itemAudio = this;
        }
    }

    public void StopSFX(bool resume)
    {
        if(pickupWwiseSFX != null)
        {
            Debug.Log("StopSFX");
            pickupWwiseSFX.Stop(AudioManager.Instance.gameObject);
            isSoundPlaying = false;
            AudioManager.Instance.itemAudio = null;
        }
    }
    private void CallBackFunction(object in_cookie, AkCallbackType callType, object in_info)
    {
        if (callType == AkCallbackType.AK_EndOfEvent)
        {
            // return if unnatural stop / already stopped
            if(!isSoundPlaying) { return; }
            // stop
            StopSFX(true);
        }
    }
    public void LoadData(GameData data)
    {
        if(data.itemInScene.TryGetValue(id, out bool obtained))
        {
            this.obtained = obtained;
            if (this == null) { return; }
            if (obtained) { Destroy(gameObject); }
            gameObject.SetActive(true);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.itemInScene.ContainsKey(id)) { data.itemInScene.Remove(id); }
        data.itemInScene[id] = obtained;
        if(this == null) { return; }
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
