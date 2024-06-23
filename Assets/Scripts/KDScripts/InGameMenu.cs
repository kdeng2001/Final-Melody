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
        PlayerInput playerInput = Player.Instance.GetComponent<PlayerInput>();
        InputAction toggleMenu = playerInput.actions["ToggleMenu"];
        toggleMenu.started += context => ToggleMenu(context);
        saveAndQuit.SetActive(false);
        inventoryUI.SetActive(false);
        pointsUI.SetActive(false);
    }

    //private void OnDestroy()
    //{
    //    PlayerInput playerInput = Player.Instance.GetComponent<PlayerInput>();
    //    InputAction toggleMenu = playerInput.actions["ToggleMenu"];
    //    toggleMenu.started -= context => ToggleMenu(context);
    //}
    public void ToggleMenu(CallbackContext context)
    {
        // cannot toggle menu while in dialogue
        if(DialogueManager.Instance.dialogueIsPlaying) { return; }

        // if menu currently active, become inactive and unpause
        gameObject.SetActive(true);
        saveAndQuit.SetActive(true);
        inventoryUI.SetActive(true);
        pointsUI.SetActive(true);

        if (isToggledOn)
        {
            //Time.timeScale = 1;
            player.UnPauseMovement();
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

        //if (saveAndQuit.activeSelf) 
        //{ 
        //    Time.timeScale = 1;
        //    isToggledOn = false;
        //    animator.Play("MenuOff");
        //}
        //// else become active and pause
        //else 
        //{ 
        //    Time.timeScale = 0; 
        //    isToggledOn = true;
        //    animator.Play("MenuOn");
        //}
        //gameObject.SetActive(!gameObject.activeSelf);
        //saveAndQuit.SetActive(!saveAndQuit.activeSelf);
        //inventoryUI.SetActive(!inventoryUI.activeSelf);
        //pointsUI.SetActive(!pointsUI.activeSelf);

    }

    public void Save()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
