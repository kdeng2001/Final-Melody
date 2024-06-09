using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public static Player Instance { get; private set; }
    [SerializeField] private Transform model;
    [SerializeField] private Transform controller;
    [SerializeField] private Transform interactor;

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

    public void SaveData(ref GameData data)
    {
        data.playerPosition = controller.position;
    }
}
