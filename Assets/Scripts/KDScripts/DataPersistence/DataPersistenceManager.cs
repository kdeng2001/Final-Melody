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
    [Header("Debugging")]
    [Tooltip("Use for testing from an individual scene without starting from main menu. Set true to load the scene with fresh data.")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string globalSaveName;
    [SerializeField] private string localSaveName;
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
            globalSaveDataHandler = new FileDataHandler(Application.persistentDataPath, globalSaveName);
            localSaveDataHandler = new FileDataHandler(Application.persistentDataPath, localSaveName);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadScene;
        //SceneManager.sceneUnloaded += SaveScene;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadScene;
        //SceneManager.sceneUnloaded -= SaveScene;
    }

    public void NewGame() 
    { 
        globalGameData = new GameData();
        localGameData = new GameData();
    }
    public void LoadGame() 
    {
        //Debug.Log("LoadGame...");
        // load any saved data from a file using the data handler
        globalGameData = globalSaveDataHandler.Load();
        localGameData = globalGameData;
        if(globalGameData == null) 
        {
            Debug.Log("No data was found");
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
        Debug.Log("FinishLoadGame...");
    }    
    public void SaveGame() 
    {
        //Debug.Log("SaveGame...");
        globalGameData = localGameData;
        // first save the current scene
        globalGameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // pass in data from scripts that implement IDataPersistence to globalGameData
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(/*ref */globalGameData);
        }
        globalSaveDataHandler.Save(globalGameData);
        Debug.Log("FinishSaveGame...");
    }

    public void LoadScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LoadScene..." + SceneManager.GetActiveScene().name);
        if (localGameData == null && initializeDataIfNull)
        {
            Debug.Log("LoadScene: localData is null");
            localGameData = new GameData(); 
        }
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        //int index = 0;
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(localGameData);
            //Debug.Log("scene data is loaded" + index++);
        }
        Debug.Log("FinishLoadScene..." + SceneManager.GetActiveScene().name);
    }
    
    public void SaveScene(Scene scene)
    {
        Debug.Log("SaveScene..." + SceneManager.GetActiveScene().name);
        if (localGameData == null && initializeDataIfNull) 
        {
            Debug.Log("SaveScene: localData is null");
            localGameData = new GameData(); 
        }
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        int index = 0;
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(/*ref */localGameData);
            Debug.Log("scene data is saved " + index++);
        }
        localSaveDataHandler.Save(localGameData);
        Debug.Log("FinishSaveScene..." + SceneManager.GetActiveScene().name);
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
        //dataPersistenceObjects = FindAllDataPersistenceObjects();
        //if(!enableDataPersistence) { return; }
        //LoadGame();
    }
    public void OnApplicationQuit()
    {

    }
    public void Quit()
    {
        Application.Quit();
    }
}
