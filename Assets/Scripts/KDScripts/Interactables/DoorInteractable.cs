using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private string sceneToEnterName;
    [SerializeField] public string entranceID;
    [SerializeField] public Transform positionToEnter;

    [SerializeField] public bool isARealDoor = true;
    public override void OnFinishInteract(){}

    public override void OnStartInteract()
    {
        SceneTransitionManager.Instance.EnterNewScene(sceneToEnterName, entranceID, isARealDoor);
    }
}
