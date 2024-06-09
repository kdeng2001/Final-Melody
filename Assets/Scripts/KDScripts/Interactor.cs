using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Interactor : MonoBehaviour
{
    [SerializeField] private string[] InteractableTags;
    private Interactable interactable;


    public void PressInteract(CallbackContext context)
    {
        if(interactable == null) { return; }
        interactable.OnStartInteract();
    }
    public void PausePlayer() { }

    private void OnTriggerEnter(Collider other)
    {
        if(HasInteractableTag(other)) 
        {
            // display interactable icon
            // enable interaction?
            Debug.Log(other.gameObject.name + " is interactable");
            if(interactable != null) { interactable.DisableInteraction(); }
            interactable = other.GetComponent<Interactable>();
            if(interactable == null) { return; }
            interactable.EnableInteraction();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(HasInteractableTag(other))
        {
            if(interactable == other.GetComponent<Interactable>()) 
            {
                interactable.DisableInteraction();
                interactable = null;
            }
        }   
    }
    private bool HasInteractableTag(Collider other) 
    {
        foreach(string tag in InteractableTags)
        {
            if(other.CompareTag(tag)) { return true; }
        }
        return false;
    }    
    private void OnEnable()
    {
        if(transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction interactAction = playerInput.actions["Interact"];
            if(interactAction != null)
            {
                interactAction.started += context => PressInteract(context);
                interactAction.canceled -= context => PressInteract(context);
            }
        }
    }
    private void OnDisable()
    {
        if (transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction interactAction = playerInput.actions["Interact"];
            if (interactAction != null)
            {
                interactAction.started -= context => PressInteract(context);
                interactAction.canceled -= context => PressInteract(context);
            }
        }
    }
}
