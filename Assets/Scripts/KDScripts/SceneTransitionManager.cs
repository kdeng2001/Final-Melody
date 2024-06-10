using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    private Player player;
    private string entranceID = "";
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
    private void Start()
    {
        SceneManager.sceneLoaded += MovePlayerToEntrance;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= MovePlayerToEntrance;
    }
    public void EnterNewScene(string sceneName, string entranceID)
    {
        DataPersistenceManager.Instance.SaveScene();
        //DataPersistenceManager.Instance.SaveGame();
        this.entranceID = entranceID;
        SceneManager.LoadScene(sceneName);
    }

    private void MovePlayerToEntrance(Scene scene, LoadSceneMode mode)
    {
        DataPersistenceManager.Instance.LoadScene();
        //DataPersistenceManager.Instance.LoadGame();

        if (entranceID == "") { return; }
        player = FindObjectOfType<Player>().GetComponent<Player>();
        DoorInteractable[] doors = FindObjectsOfType<DoorInteractable>();
        foreach (DoorInteractable door in doors)
        {
            if (entranceID == door.entranceID)
            {
                player.SetPosition(door.positionToEnter);    
                entranceID = ""; 
                break;   
                //player.SetPosition(door.transform.position); break;
            }
        }
    }

    public void LoadScene() { }
    public void SaveScene() { }
}
