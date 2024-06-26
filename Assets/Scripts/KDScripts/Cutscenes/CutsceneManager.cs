using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    [SerializeField] private Cutscene[] cutscenes;

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
        SceneManager.sceneLoaded += HandleCutscenes;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleCutscenes;
    }

    public void HandleCutscenes(Scene scene, LoadSceneMode mode)
    {        
        Debug.Log("handling cutscene: " + scene.name);

        foreach(Cutscene cutscene in cutscenes) 
        {
            Debug.Log(cutscene.sceneName + " = " + SceneManager.GetActiveScene().name);
            if(cutscene.sceneName == SceneManager.GetActiveScene().name && !cutscene.isComplete) 
            { 
                if(!cutscene.playOnStart) { continue; }
                cutscene.Play();
                break;
            }
        }
    }

}


public struct CutsceneData
{
    // id to link cutscene data to corresponding cutscene
    public string cutsceneID;
    // order in which this cutscene part plays
    public int cutsceneOrder;
    // actors in this cutscene part
    public List<CutsceneActor> actors;
    // dialogue a cutscene actor triggers
    public TextAsset dialogue;
}
