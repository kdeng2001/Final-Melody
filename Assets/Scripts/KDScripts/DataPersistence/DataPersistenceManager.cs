using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    private List<IDataPersistence> dataPersistenceObjects;
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
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.LoadData(gameData);
        }
    }    
    public void SaveGame() 
    {
        // pass in data from scripts that implement IDataPersistence to GameData
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if (dataPersistenceObj == null) { continue; }
            dataPersistenceObj.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
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
        if(!enableDataPersistence) { return; }
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
