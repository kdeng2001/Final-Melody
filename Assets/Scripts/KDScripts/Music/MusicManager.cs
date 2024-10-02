using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    //[SerializeField] public AK.Wwise.Event musicEvent;
    //[SerializeField] BattleMusicState[] battleMusic;
    //[SerializeField] WorldMusicState[] worldMusic;

    //private string currentStateName;
    //private string currentStateGroup;

    //private WorldMusicState currentWorldMusic;

    //public static MusicManager Instance;
    //private void Awake()
    //{
    //    if(Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        musicEvent.Post(gameObject);
    //        //musicEvent.Post(gameObject);
    //        PlaySceneMusic(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    //    }
    //}

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += PlaySceneMusic;
    //    SceneManager.sceneLoaded += PlayBattleMusic;
    //    SceneManager.sceneUnloaded += EndBattleMusic;
    //}
    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= PlaySceneMusic;
    //    SceneManager.sceneLoaded -= PlayBattleMusic;
    //    SceneManager.sceneUnloaded -= EndBattleMusic;
    //}
    //public void PlaySceneMusic(Scene scene, LoadSceneMode mode)
    //{
    //    //Debug.Log(scene.name + " mode: " + mode);
    //    //if(LoadSceneMode.Additive == mode) { Debug.Log(scene.name + " mode: " + mode + " will not play"); return; }
    //    // loop to find corresponding scene
    //    foreach (WorldMusicState state in worldMusic)
    //    {
    //        foreach (string sceneName in state.sceneNames)
    //        {
    //            // when scene found
    //            if (sceneName == scene.name)
    //            {
    //                // if music is unavailable
    //                if (state.stateName == null) { continue; }
    //                // if the current scene's music is same as new scene's music do nothing and return
    //                // otherwise, play new music and stop old one
    //                if (state.stateName != currentStateName)
    //                {
    //                    currentStateName = state.stateName;
    //                    currentStateGroup = state.stateGroup;
    //                    AKRESULT result = AkSoundEngine.SetState(currentStateGroup, currentStateName);
    //                    if (result != AKRESULT.AK_Success)
    //                    {
    //                        Debug.LogError($"SetState failed: {result}");
    //                    }
    //                    else
    //                    {
    //                        Debug.Log($"SetState succeeded: {state.stateGroup} -> {state.stateName}");
    //                    }
    //                    currentWorldMusic = state;
    //                    Debug.Log(currentStateGroup + ", " + currentStateName);
    //                }
    //                return;
    //            }
    //        }
    //    }

    //}
    //public void PlayBattleMusic(Scene scene, LoadSceneMode mode)
    //{
    //    if (LoadSceneMode.Additive != mode) { return; }
    //    foreach (BattleMusicState state in battleMusic)
    //    {
    //        foreach (string sceneName in state.sceneNames)
    //        {
    //            // when scene found
    //            if (sceneName == scene.name)
    //            {
    //                // if music is unavailable
    //                if (state.stateName == null) { continue; }
    //                // if the current scene's music is same as new scene's music do nothing and return
    //                // otherwise, play new music and stop old one
    //                if (state.stateName != currentStateName)
    //                {
    //                    currentStateName = state.stateName;
    //                    currentStateGroup = state.stateGroup;
    //                    AKRESULT result = AkSoundEngine.SetState(currentStateGroup, currentStateName);
    //                    if (result != AKRESULT.AK_Success)
    //                    {
    //                        Debug.LogError($"SetState failed: {result}");
    //                    }
    //                    else
    //                    {
    //                        Debug.Log($"SetState succeeded: {state.stateGroup} -> {state.stateName}");
    //                    }
    //                    Debug.Log(currentStateGroup + ", " + currentStateName);
    //                }
    //                return;
    //            }
    //        }
    //    }
    //}
    //public void EndBattleMusic(Scene scene)
    //{
    //    foreach (BattleMusicState state in battleMusic)
    //    {
    //        foreach (string sceneName in state.sceneNames)
    //        {
    //            if (sceneName == scene.name)
    //            {
    //                if (state.stateName == currentStateName)
    //                {
    //                    currentStateName = currentWorldMusic.stateName;
    //                    currentStateGroup = currentWorldMusic.stateGroup;
    //                    AKRESULT result = AkSoundEngine.SetState(currentStateGroup, currentStateName);
    //                    if (result != AKRESULT.AK_Success)
    //                    {
    //                        Debug.LogError($"SetState failed: {result}");
    //                    }
    //                    else
    //                    {
    //                        Debug.Log($"SetState succeeded: {state.stateGroup} -> {state.stateName}");
    //                    }
    //                    Debug.Log(currentStateGroup + ", " + currentStateName);
    //                }
    //                return;
    //            }
    //        }
    //    }
    //}

}



//[System.Serializable]
//public struct BattleMusicState
//{
//    public string[] sceneNames;
//    public string stateName;
//    public string stateGroup;
//}


//[System.Serializable]
//public struct WorldMusicState
//{
//    public string[] sceneNames;
//    public string stateName;
//    public string stateGroup;
//}