using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnEssentials : MonoBehaviour
{
    public static SpawnEssentials Instance;
    [SerializeField] private bool spawnEssentialsAtStart = false;

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
            if(spawnEssentialsAtStart) 
            {
                Debug.Log("Spawning essentials...");
                SceneManager.LoadScene("KDEssentials", LoadSceneMode.Additive);
                Debug.Log("Finish spawning essentials!");
            }
        }

    }
    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += SetUpEssentials;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded += SetUpEssentials;
    //}


    //private void SetUpEssentials(Scene scene, LoadSceneMode mode)
    //{
    //    GameObject.
    //}
}
