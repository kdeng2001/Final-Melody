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
                cutscene.Play();
                break;
            }
        }
    }

}
