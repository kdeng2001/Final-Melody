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
                //currentContainer.data.pauseMusic?.Invoke();
                Debug.Log("AudioManager pause current");
                currentContainer.PauseMusic();
            }
            // end current, play new
            else if(currentContainer.type == container.type)
            {
                //currentContainer.data.endMusic?.Invoke();
                Debug.Log("AudioManager destroy current");
                currentContainer.DestroyContainer();
            }
        }
        currentContainer = container;
        currentID = container.id;
        //currentContainer.data.playMusic?.Invoke();
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
            //currentContainer.data.endMusic?.Invoke();
            currentContainer.DestroyContainer();
            currentContainer = next;
            currentID = ID;
            //currentContainer.data.playMusic?.Invoke();
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

// ================================================================================================= OLD ===========================================================================

//    public static AudioManager Instance;
//    [SerializeField] private WorldMusicLoops[] worldMusic;
//    [SerializeField] private BattleMusicLoops[] battleMusic;
//    //public AK.Wwise.Event[] musicLoops;
//    //public string[] sceneName;
//    private uint currentMusicID = 0;
//    private string currentLoopName = "";

//    private AK.Wwise.Event currentWorldMusic;
//    private AK.Wwise.Event currentBattleMusic;

//    public Item itemAudio;
//    private void Awake()
//    {
//        if(Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//    }

//    private void OnEnable()
//    {
//        SceneManager.sceneLoaded += PlaySceneMusic;
//        SceneManager.sceneLoaded += PlayBattleMusic;
//        SceneManager.sceneUnloaded += EndBattleMusic;
//    }
//    private void OnDisable()
//    {
//        SceneManager.sceneLoaded -= PlaySceneMusic;
//        SceneManager.sceneLoaded -= PlayBattleMusic;
//        SceneManager.sceneUnloaded -= EndBattleMusic;
//    }
//    public void PlaySceneMusic(Scene scene, LoadSceneMode mode)
//    {
//        if(itemAudio != null) { itemAudio.StopSFX(true); }
//        //Debug.Log(scene.name + " mode: " + mode);
//        //if(LoadSceneMode.Additive == mode) { Debug.Log(scene.name + " mode: " + mode + " will not play"); return; }
//        // loop to find corresponding scene
//        foreach (WorldMusicLoops loop in worldMusic)
//        {
//            foreach (string sceneName in loop.sceneNames)
//            {
//                // when scene found
//                if (sceneName == scene.name)
//                {
//                    // if music is unavailable
//                    if(loop.musicLoop == null) { continue; }
//                    // if the current scene's music is same as new scene's music do nothing and return
//                    // otherwise, play new music and stop old one
//                    if (loop.musicLoop.Name != currentLoopName)
//                    {
//                        if (currentMusicID != 0) AkSoundEngine.StopPlayingID(currentMusicID);
//                        Debug.Log("Playing: " + loop.musicLoop.Id);
//                        //currentMusicID = loop.musicLoop.Post(Player.Instance.controller.gameObject);
//                        currentMusicID = loop.musicLoop.Post(gameObject);

//                        currentLoopName = loop.musicLoop.Name;
//                        currentWorldMusic = loop.musicLoop;
//                    }
//                    return;
//                }
//            }
//        }

//    }
//    public void PlayBattleMusic(Scene scene, LoadSceneMode mode)
//    {
//        if (itemAudio != null) { itemAudio.StopSFX(true); }
//        if (LoadSceneMode.Additive != mode) { return; }
//        foreach (BattleMusicLoops loop in battleMusic)
//        {
//            foreach (string sceneName in loop.sceneNames)
//            {
//                // when scene found
//                if (sceneName == scene.name)
//                {
//                    // if music is unavailable
//                    if (loop.musicLoop == null) { continue; }
//                    // if the current scene's music is same as new scene's music do nothing and return
//                    // otherwise, play new music and stop old one
//                    if (loop.musicLoop.Name != currentLoopName)
//                    {
//                        if( currentMusicID != 0) { AkSoundEngine.StopPlayingID(currentMusicID); }
//                        //currentMusicID = loop.musicLoop.Post(Player.Instance.controller.gameObject);
//                        currentMusicID = loop.musicLoop.Post(gameObject);

//                        currentLoopName = loop.musicLoop.Name;
//                        currentBattleMusic = loop.musicLoop;
//                    }
//                    return;
//                }
//            }
//        }
//    }
//    public void EndBattleMusic(Scene scene)
//    {
//        foreach (BattleMusicLoops loop in battleMusic)
//        {
//            foreach(string sceneName in loop.sceneNames)
//            {
//                if(sceneName == scene.name)
//                {
//                    if(loop.musicLoop.Name == currentLoopName)
//                    {
//                        if(currentMusicID != 0) { AkSoundEngine.StopPlayingID(currentMusicID); }
//                        //currentMusicID = currentWorldMusic.Post(Player.Instance.controller.gameObject);
//                        currentMusicID = currentWorldMusic.Post(gameObject);

//                        currentLoopName = currentWorldMusic.Name;
//                        //currentWorldMusic = currentWorldMusic;
//                    }
//                    return;
//                }
//            }
//        }
//    }

//    private bool currentMusicIsPlaying = true;
//    public void PauseCurrentMusic()
//    {
//        if(!currentMusicIsPlaying) { return; }
//        AkSoundEngine.StopPlayingID(currentMusicID);
//        currentMusicIsPlaying = false;
//    }

//    public void ResumeCurrentMusic() 
//    {
//        if (currentMusicIsPlaying) { return; }
//        AkSoundEngine.StopPlayingID(currentMusicID);
//        currentMusicID = currentWorldMusic.Post(gameObject);
//        currentMusicIsPlaying = true;
//    }
//}

//[System.Serializable]
//public struct WorldMusicLoops
//{
//    public string[] sceneNames;
//    public AK.Wwise.Event musicLoop;
//}

//[System.Serializable]
//public struct BattleMusicLoops
//{
//    public string[] sceneNames;
//    public AK.Wwise.Event musicLoop;
//}
