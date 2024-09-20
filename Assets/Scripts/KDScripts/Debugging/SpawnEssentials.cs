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
                SceneManager.LoadScene("KDEssentials", LoadSceneMode.Additive);
            }
        }

    }
}
