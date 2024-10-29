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
    public GameData globalGameData { get; private set; }
    public GameData localGameData { get; private set; }
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
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadScene;
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
        globalGameData = localGameData;
        // first save the current scene
        globalGameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // pass in data from scripts that implement IDataPersistence to globalGameData
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(globalGameData);
        }
        globalSaveDataHandler.Save(globalGameData);
    }

    public void LoadScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MockBattleScene" || scene.name == "VirtualRook_jyj_KD_BattleStage") { return; }

        if (localGameData == null && initializeDataIfNull)
        {
            localGameData = new GameData(); 
        }

        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(localGameData);
        }
    }
    
    public void SaveScene(Scene scene)
    {
        if (localGameData == null && initializeDataIfNull) 
        {
            localGameData = new GameData(); 
        }

        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(localGameData);
        }
        localSaveDataHandler.Save(localGameData);
    }
    public List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public void OnApplicationQuit()
    {

    }
    public void Quit()
    {
        Application.Quit();
    }
}
