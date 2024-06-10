using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
/// <summary>
/// Stores GameData
/// Propagates GameData to classes that implement IDataPersistence to load data
/// </summary>
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool enableDataPersistence = true;
    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;
    public List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
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

    public void NewGame() { gameData = new GameData(); }
    public void LoadGame() 
    {
        // load any saved data from a file using the data handler
        gameData = fileDataHandler.Load();
        if(gameData == null) 
        {
            Debug.Log("No data was found");
            NewGame();
            return;
        }
        // else load data for all scripts that implement IDataPersistence
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(gameData);
        }
    }    
    public void SaveGame() 
    {
        Debug.Log("SaveGame...");
        // pass in data from scripts that implement IDataPersistence to GameData
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }

    public void LoadScene()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(gameData);
        }
    }
    
    public void SaveScene()
    {
        Debug.Log("SaveScene...");
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }


    public List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }



    // --- simple save on quit, load on start ---
    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        //SceneManager.sceneLoaded += GetDataPersistenceObjectsInScene;
        if(!enableDataPersistence) { return; }
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        //SceneManager.sceneLoaded -= GetDataPersistenceObjectsInScene;
        SaveGame();
    }

    //public void GetDataPersistenceObjectsInScene(Scene scene, LoadSceneMode mode)
    //{
    //    dataPersistenceObjects = FindAllDataPersistenceObjects();
    //}
}
