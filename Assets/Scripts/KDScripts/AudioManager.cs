using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AK.Wwise;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private WorldMusicLoops[] worldMusic;
    [SerializeField] private BattleMusicLoops[] battleMusic;
    //public AK.Wwise.Event[] musicLoops;
    //public string[] sceneName;
    private uint currentMusicID = 0;
    private string currentLoopName = "";

    private AK.Wwise.Event currentWorldMusic;
    private AK.Wwise.Event currentBattleMusic;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PlaySceneMusic;
        SceneManager.sceneLoaded += PlayBattleMusic;
        SceneManager.sceneUnloaded += EndBattleMusic;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlaySceneMusic;
        SceneManager.sceneLoaded -= PlayBattleMusic;
        SceneManager.sceneUnloaded -= EndBattleMusic;
    }
    public void PlaySceneMusic(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + " mode: " + mode);
        //if(LoadSceneMode.Additive == mode) { Debug.Log(scene.name + " mode: " + mode + " will not play"); return; }
        // loop to find corresponding scene
        foreach (WorldMusicLoops loop in worldMusic)
        {
            foreach (string sceneName in loop.sceneNames)
            {
                // when scene found
                if (sceneName == scene.name)
                {
                    // if music is unavailable
                    if(loop.musicLoop == null) { continue; }
                    // if the current scene's music is same as new scene's music do nothing and return
                    // otherwise, play new music and stop old one
                    if (loop.musicLoop.Name != currentLoopName)
                    {
                        if (currentMusicID != 0) AkSoundEngine.StopPlayingID(currentMusicID);
                        Debug.Log("Playing: " + loop.musicLoop.Id);
                        currentMusicID = loop.musicLoop.Post(Player.Instance.controller.gameObject);
                        currentLoopName = loop.musicLoop.Name;
                        currentWorldMusic = loop.musicLoop;
                    }
                    return;
                }
            }
        }

    }
    public void PlayBattleMusic(Scene scene, LoadSceneMode mode)
    {
        if(LoadSceneMode.Additive != mode) { return; }
        foreach (BattleMusicLoops loop in battleMusic)
        {
            foreach (string sceneName in loop.sceneNames)
            {
                // when scene found
                if (sceneName == scene.name)
                {
                    // if music is unavailable
                    if (loop.musicLoop == null) { continue; }
                    // if the current scene's music is same as new scene's music do nothing and return
                    // otherwise, play new music and stop old one
                    if (loop.musicLoop.Name != currentLoopName)
                    {
                        if( currentMusicID != 0) { AkSoundEngine.StopPlayingID(currentMusicID); }
                        currentMusicID = loop.musicLoop.Post(Player.Instance.controller.gameObject);
                        currentLoopName = loop.musicLoop.Name;
                        currentBattleMusic = loop.musicLoop;
                    }
                    return;
                }
            }
        }
    }
    public void EndBattleMusic(Scene scene)
    {
        foreach (BattleMusicLoops loop in battleMusic)
        {
            foreach(string sceneName in loop.sceneNames)
            {
                if(sceneName == scene.name)
                {
                    if(loop.musicLoop.Name == currentLoopName)
                    {
                        if(currentMusicID != 0) { AkSoundEngine.StopPlayingID(currentMusicID); }
                        currentMusicID = currentWorldMusic.Post(Player.Instance.controller.gameObject);
                        currentLoopName = currentWorldMusic.Name;
                        //currentWorldMusic = currentWorldMusic;
                    }
                    return;
                }
            }
        }
    }
}

[System.Serializable]
public struct WorldMusicLoops
{
    public string[] sceneNames;
    public AK.Wwise.Event musicLoop;
}

[System.Serializable]
public struct BattleMusicLoops
{
    public string[] sceneNames;
    public AK.Wwise.Event musicLoop;
}
