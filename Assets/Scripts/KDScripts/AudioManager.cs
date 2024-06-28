using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AK.Wwise;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private WorldMusicLoops[] worldMusic;
    //public AK.Wwise.Event[] musicLoops;
    //public string[] sceneName;
    private uint currentWorldMusicID = 0;
    private string currentLoopName = "";
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
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlaySceneMusic;
    }
    //public void PlaySceneMusic(Scene scene, LoadSceneMode mode)
    //{
    //    for(int i=0; i<musicLoops.Length; i++)
    //    {
    //        if(sceneName[i] == scene.name)
    //        {
    //            AkSoundEngine.StopPlayingID(currentWorldMusicID);
    //            currentWorldMusicID = musicLoops[i].Post(gameObject);
    //        }
    //    }
    //}
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

                        if (currentWorldMusicID != 0) AkSoundEngine.StopPlayingID(currentWorldMusicID);
                        Debug.Log("Playing: " + loop.musicLoop.Id);
                        currentWorldMusicID = loop.musicLoop.Post(Player.Instance.controller.gameObject);
                        currentLoopName = loop.musicLoop.Name;

                    }
                    return;
                }
            }
        }

    }
    public void PlayBattleSceneMusic(Scene scene, LoadSceneMode mode)
    {
        foreach (WorldMusicLoops loop in worldMusic)
        {
            foreach(string sceneName in loop.sceneNames)
            {
                if(scene.name == sceneName)
                {
                    loop.musicLoop.Post(Player.Instance.controller.gameObject);
                }
            }
        }
    }
    public void StopBattleSceneMusic(Scene scene)
    {

    }
}

[System.Serializable]

public struct WorldMusicLoops
{
    public string[] sceneNames;
    public AK.Wwise.Event musicLoop;
}
