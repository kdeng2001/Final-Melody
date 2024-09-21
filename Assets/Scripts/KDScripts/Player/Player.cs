using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDataPersistence
{
    public static Player Instance { get; private set; }
    [SerializeField] public Transform model;
    [SerializeField] public Transform controller;
    [SerializeField] public Transform interactor;
    [SerializeField] public PlayerInput playerInput;

    private Interactor interactorComponent;
    CharacterMovement movement;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            interactorComponent = interactor.GetComponent<Interactor>();
            movement = GetComponentInChildren<CharacterMovement>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PauseMovement() { movement.pauseMovement = true; }
    public void UnpauseMovement() { movement.pauseMovement = false; }
    public void PauseInteractor() { interactorComponent.enabled = false; }
    public void UnpauseInteractor() { interactorComponent.enabled = true; }
    public void SetPosition(Transform t)
    {
        CharacterController c = controller.GetComponent<CharacterController>();
        c.enabled = false;
        controller.position = model.position = interactor.position = t.position;
        c.enabled = true;
    }

    public void LoadData(GameData data)
    {
        CharacterController c = controller.GetComponent<CharacterController>();
        c.enabled = false;
        controller.position = model.position = interactor.position = data.playerPosition;
        c.enabled = true;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = controller.position;
    }

    private void OnEnable()
    {
        InputAction toggleMenu = playerInput.actions["ToggleMenu"];
        toggleMenu.started += context => InGameMenu.Instance.ToggleMenu(context);
    }
    private void OnDisable()
    {
        InputAction toggleMenu = playerInput.actions["ToggleMenu"];
        toggleMenu.started -= context => InGameMenu.Instance.ToggleMenu(context);
    }
}
