using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    private Player player;
    private string entranceID = "";
    private Coroutine EnterSceneCoroutine = null;

    [SerializeField] public AK.Wwise.Event openDoorSound;
    [SerializeField] public AK.Wwise.Event closeDoorSound;
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
            player = FindObjectOfType<Player>().GetComponent<Player>();
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
    public void EnterNewScene(string sceneName, string entranceID, bool isRealDoor)
    {
        if(EnterSceneCoroutine != null) { return; }
        DataPersistenceManager.Instance.SaveScene(SceneManager.GetActiveScene());
        this.entranceID = entranceID;
        //SceneManager.LoadScene(sceneName);
        EnterSceneCoroutine = StartCoroutine(EnterScene(sceneName, isRealDoor));
        //Debug.Log("Load scene from SceneTransitionManager " + SceneManager.GetActiveScene().name);
    }

    private IEnumerator EnterScene(string sceneName,  bool isRealDoor)
    {
        
        LoadSceneManager.Instance.FadeToScreen(LoadSceneManager.Instance.blackScreen);
        player.PauseMovement();
        if(isRealDoor) { /*openDoorSound.Post(AudioManager.Instance.gameObject);*/ }
        yield return new WaitForSeconds(1.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
        }
        LoadSceneManager.Instance.FadeFromScreen(LoadSceneManager.Instance.blackScreen);
        if (isRealDoor) { /*closeDoorSound.Post(AudioManager.Instance.gameObject);*/ }
        player.UnpauseMovement();
        EnterSceneCoroutine = null;
    }

    private void MovePlayerToEntrance(Scene scene, LoadSceneMode mode)
    {
        //DataPersistenceManager.Instance.LoadScene();

        if (entranceID == "") { Debug.Log("no entrance id"); return; }
        //Debug.Log("finding door...");
        //player = FindObjectsOfType<Player>().GetComponents<Player>();
        DoorInteractable[] doors = FindObjectsOfType<DoorInteractable>();
        foreach (DoorInteractable door in doors)
        {
            if (entranceID == door.entranceID)
            {
                player.SetPosition(door.positionToEnter);
                //Debug.Log("Move player to entrance");
                entranceID = ""; 
                break;   
                //player.SetPosition(door.transform.position); break;
            }
        }
    }

    public void LoadScene() { }
    public void SaveScene() { }
}
