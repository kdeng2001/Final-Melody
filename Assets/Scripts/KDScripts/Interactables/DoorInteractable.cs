using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private string sceneToEnterName;
    [SerializeField] public string entranceID;
    [SerializeField] public Transform positionToEnter;
    public override void OnFinishInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStartInteract()
    {
        SceneTransitionManager.Instance.EnterNewScene(sceneToEnterName, entranceID);
    }
}