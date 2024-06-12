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
    [SerializeField] private string globalSaveName;
    [SerializeField] private string localSaveName;
    [SerializeField] private bool enableDataPersistence = true;
    public static DataPersistenceManager Instance { get; private set; }
    private GameData globalGameData;
    private GameData localGameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler globalSaveDataHandler;
    private FileDataHandler localSaveDataHandler;
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

    public void NewGame() 
    { 
        globalGameData = new GameData();
        localGameData = new GameData();
    }
    public void LoadGame() 
    {
        // load any saved data from a file using the data handler
        globalGameData = globalSaveDataHandler.Load();
        localGameData = globalGameData;
        if(globalGameData == null) 
        {
            Debug.Log("No data was found");
            NewGame();
            return;
        }
        // else load data for all scripts that implement IDataPersistence

        // first load the correct scene
        SceneManager.LoadScene(globalGameData.sceneIndex);
        // then load everything else
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(globalGameData);
        }
    }    
    public void SaveGame() 
    {
        Debug.Log("SaveGame...");
        globalGameData = localGameData;
        // first save the current scene
        globalGameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // pass in data from scripts that implement IDataPersistence to globalGameData
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(ref globalGameData);
        }
        globalSaveDataHandler.Save(globalGameData);
    }

    public void LoadScene()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(localGameData);
        }
    }
    
    public void SaveScene()
    {
        Debug.Log("SaveScene...");
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(ref localGameData);
        }
        localSaveDataHandler.Save(localGameData);
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
        globalSaveDataHandler = new FileDataHandler(Application.persistentDataPath, globalSaveName);
        localSaveDataHandler = new FileDataHandler(Application.persistentDataPath, localSaveName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        if(!enableDataPersistence) { return; }
        LoadGame();
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
