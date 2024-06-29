using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
    [Tooltip("an icon that appears, showing that the object is interactable currently")]
    [SerializeField] private GameObject InteractableIcon;
    private bool _canInteract = false;
    public bool interacting = false;
    public virtual void Start()
    {
        DisableInteraction();
    }
    public abstract void OnStartInteract();
    public abstract void OnFinishInteract();
    public void EnableInteraction() 
    {
        _canInteract = true;
        DisplayIcon();
    }
    public void DisableInteraction()
    {
        _canInteract = false;
        RemoveIcon();
    }
    private void DisplayIcon() { InteractableIcon.SetActive(true); }
    private void RemoveIcon() { InteractableIcon.SetActive(false); }
}
