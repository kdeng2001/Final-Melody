using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public static Player Instance { get; private set; }
    [SerializeField] public Transform model;
    [SerializeField] public Transform controller;
    [SerializeField] public Transform interactor;

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
    public void UnPauseMovement() { movement.pauseMovement = false; }
    public void PauseInteractor() { interactorComponent.enabled = false; Debug.Log("pause interactor"); }
    public void UnpauseInteractor() { interactorComponent.enabled = true; Debug.Log("unpause interactor");}
    public void SetPosition(Transform t)
    {
        CharacterController c = controller.GetComponent<CharacterController>();
        c.enabled = false;
        controller.position = model.position = interactor.position = t.position;
        c.enabled = true;
        //Debug.Log("Finish set player position");
    }

    public void LoadData(GameData data)
    {
        //Debug.Log("Load player data");
        CharacterController c = controller.GetComponent<CharacterController>();
        c.enabled = false;
        controller.position = model.position = interactor.position = data.playerPosition;
        c.enabled = true;
        //Debug.Log("Finish load player data");
    }

    public void SaveData(/*ref */GameData data)
    {
        //Debug.Log("Save player data");
        data.playerPosition = controller.position;
    }
}
