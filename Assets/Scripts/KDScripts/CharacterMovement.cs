using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class CharacterMovement : Movement
{
    public AK.Wwise.Event footsteps;
    private float timeBetweenFootsteps = .35f;
    Coroutine footstepCoroutine;

    public bool pauseMovement = false;
    private void OnEnable()
    {
        if (transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed += context => SetDirection(context);
                moveAction.canceled += context => SetDirection(context);
            }
        }
    }
    private void OnDisable()
    {
        if (transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed -= context => SetDirection(context);
                moveAction.canceled -= context => SetDirection(context);
            }
        }
    }

    public override void Update()
    {
        if (pauseMovement) { return; }
        base.Update();
        if (direction != Vector2.zero) { PlayFootstepSound(); }
        else 
        { 
            footsteps.Stop(gameObject);
            footstepCoroutine = null;
        }
    }

    public void PlayFootstepSound()
    {
        if(footstepCoroutine == null) { footstepCoroutine = StartCoroutine(FootstepHandler(timeBetweenFootsteps)); }
    }

    IEnumerator FootstepHandler(float timeBetweenFootsteps)
    {
        footsteps.Post(gameObject);
        Debug.Log("play footstep");
        yield return new WaitForSeconds(timeBetweenFootsteps);

        footstepCoroutine = null;
    }
}
