using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InGameMenu : MonoBehaviour
{
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
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        PlayerInput playerInput = Player.Instance.GetComponent<PlayerInput>();
        InputAction toggleMenu = playerInput.actions["ToggleMenu"];
        toggleMenu.started += context => ToggleMenu(context);
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
        if(gameObject.activeSelf) { Time.timeScale = 1; }
        // else become active and pause
        else { Time.timeScale = 0; }
        gameObject.SetActive(!gameObject.activeSelf);
    }


}
