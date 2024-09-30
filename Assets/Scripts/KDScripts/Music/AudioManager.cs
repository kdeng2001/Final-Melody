using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AK.Wwise;
using System;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    [SerializeField] public GameObject[] musicContainers;
    // states of current music
    private Dictionary<MusicType, MusicContainer> musicState;
    //private MusicData currentMusic = null;
    private MusicContainer currentContainer = null;
    private string currentID = "";
    public Item itemAudio;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicState = new Dictionary<MusicType, MusicContainer>();
            musicState[MusicType.Item] = null;
            musicState[MusicType.Overworld] = null;
            musicState[MusicType.Battle] = null;
            musicState[MusicType.Cutscene] = null;
        }
    }

    public void CreateMusic(string id)
    {

        if(currentContainer != null && currentID == id)
        {
            return; 
        }
        GameObject obj;
        foreach(GameObject o in musicContainers)
        {
            if(o.GetComponent<MusicContainer>().GetID() == id) 
            {
                obj = Instantiate(o);
                obj.transform.SetParent(transform);        
                MusicContainer container = obj.GetComponent<MusicContainer>();
                LoadMusic(container);
                Debug.Log("created music index: " + container.id);
                break;
            }
        }

    }

    // adds music data to musicState, without playing it
    public void PreLoadMusic(MusicContainer container)
    {
        // removes soundbank, destroys musicData if it exists in musicState, and sets to data
        if(musicState[container.type] != null) { musicState[container.type].DestroyContainer(); }
        musicState[container.type] = container;
    }

    public void LoadMusic(MusicContainer container)
    {
        // Preload music
        PreLoadMusic(container);
        // Battle > Cutscene > Overworld
        // preload and do nothing
        if (currentContainer != null) 
        { 
            if (currentContainer.type > container.type)
            {
                Debug.Log("AudioManager no change");
                return;
            }
            // pause current, play new
            else if(currentContainer.type < container.type)
            {
                Debug.Log("AudioManager pause current");
                currentContainer.PauseMusic();
            }
            // end current, play new
            else if(currentContainer.type == container.type)
            {
                Debug.Log("AudioManager destroy current");
                currentContainer.DestroyContainer();
            }
        }
        currentContainer = container;
        currentID = container.id;
        currentContainer.PlayMusic();
        Debug.Log("AudioManager play current");
    }



    //// can only unload current music, when current music finishes
    public void UnloadCurrentMusic()
    {
        // base case (overworld music should only unload when a new overworld music has already replaced it)
        // there should always be an overworld music loaded
        if (currentContainer.type == MusicType.Overworld)
        {
            return;
        }
        // find the next track to resume
        else
        {
            MusicContainer next = null;
            string ID = "";
            foreach (KeyValuePair<MusicType, MusicContainer> entry in musicState)
            {
                // finds the closest available music type thats less than currentMusic's type
                if (entry.Key < currentContainer.type)
                {
                    if (next == null && entry.Value != null) 
                    { 
                        next = entry.Value;
                        ID = entry.Value.id;
                    }
                    else if (entry.Key > next.type && entry.Value != null && entry.Key < currentContainer.type) 
                    { 
                        next = entry.Value;
                        ID = entry.Value.id;
                    }
                }
            }
            // end currentMusic, set and begin next currentMusic
            currentContainer.DestroyContainer();
            currentContainer = next;
            currentID = ID;
            currentContainer.PlayMusic();
        }
    }

    public void UnloadAll()
    {
        foreach(MusicType type in Enum.GetValues(typeof(MusicType)))
        {
            if (musicState[type] != null)
            {
                musicState[type].DestroyContainer();
            }
            musicState[type] = null;
        }
    }
}

public enum MusicType
{
    // manually load, default unload
    Item = 3,
    // start, default unload
    Battle = 2,
    // trigger, manually unload
    Cutscene = 1,
    // 
    Overworld = 0
}

public enum MusicLoadType
{
    Start,
    CustomCode,
}

public enum MusicUnloadType
{
    Destroy,
    CustomCode,
}