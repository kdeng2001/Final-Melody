using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject saveAndQuit;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pointsUI;
    public bool isToggledOn { get; private set; }
    private Player player;
    public static InGameMenu Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            isToggledOn = false;
            player = FindObjectOfType<Player>().GetComponent<Player>();
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {

        saveAndQuit.SetActive(false);
        inventoryUI.SetActive(false);
        pointsUI.SetActive(false);
    }
    public void ToggleMenu(CallbackContext context)
    {
        // cannot toggle menu if disabled
        if(!enabled) { return; }
        // cannot toggle menu while in dialogue
        if(DialogueManager.Instance.dialogueIsPlaying) { return; }
        // cannot toggle menu while shopping
        if(enabled == false) { return; }
        // if menu currently active, become inactive and unpause
        gameObject.SetActive(true);
        saveAndQuit.SetActive(true);
        inventoryUI.SetActive(true);
        pointsUI.SetActive(true);

        if (isToggledOn)
        {
            //Time.timeScale = 1;
            Settings.Instance.ExitSettings();
            player.UnpauseMovement();
            isToggledOn = false;
            animator.Play("MenuOff");
        }
        // else become active and pause
        else
        {
            //Time.timeScale = 0;
            player.PauseMovement();
            isToggledOn = true;
            animator.Play("MenuOn");
        }

    }
    public void Save()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void Quit()
    {
        DestroyOnMainMenu[] destroyObjects = FindObjectsOfType<DestroyOnMainMenu>();
        foreach(DestroyOnMainMenu obj in destroyObjects)
        {
            Destroy(obj.gameObject);
        }
        ToMainMenu();
        //Application.Quit();
    }
    public delegate void DestroyLoadMainMenu();
    public DestroyLoadMainMenu destroyLoadMainMenu;
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenSettings()
    {
        Settings.Instance.Popup();
    }
}
