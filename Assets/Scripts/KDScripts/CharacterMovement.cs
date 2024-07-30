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


    public delegate void OnPauseVarChange(bool val);
    public OnPauseVarChange onPauseVarChange;
    private bool _pauseMovement = false;
    public bool pauseMovement
    {
        get { return _pauseMovement; }
        set
        {
            if(_pauseMovement == value) { return; }
            _pauseMovement = value;
            onPauseVarChange?.Invoke(value);
        }
    }
    
    private void OnEnable()
    {
        onPauseVarChange += OnPauseMovementChange;
        if (transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.started += PlayFootstepSound;
                moveAction.canceled += StopFootstepSound;
                moveAction.performed += context => SetDirection(context);
                moveAction.canceled += context => SetDirection(context);
            }
            InputAction runAction = playerInput.actions["ToggleRun"];
            if(runAction != null)
            {
                runAction.started += ToggleRun;
                runAction.canceled += ToggleRun;
            }
        }
    }
    private void OnDisable()
    {
        onPauseVarChange -= OnPauseMovementChange;
        if (transform.parent.TryGetComponent(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.started -= PlayFootstepSound;
                moveAction.canceled -= StopFootstepSound;
                moveAction.performed -= context => SetDirection(context);
                moveAction.canceled -= context => SetDirection(context);
            }
            InputAction runAction = playerInput.actions["ToggleRun"];
            if (runAction != null)
            {
                runAction.started -= ToggleRun;
                runAction.canceled -= ToggleRun;
            }
        }
    }

    public override void Update()
    {
        if (pauseMovement) 
        { 
            return; 
        }
        base.Update();
    }

    public void PlayFootstepSound(CallbackContext ctx)
    {
        if(pauseMovement) { return; }
        footsteps.Post(gameObject);
    }

    public void StopFootstepSound(CallbackContext ctx)
    {
        footsteps.Stop(gameObject);
    }

    public void OnPauseMovementChange(bool val)
    {
        if(val == true) { footsteps.Stop(gameObject); animHandler.Idle(); }
        else 
        { 
            if(transform.parent.TryGetComponent(out PlayerInput playerInput) 
                && playerInput.actions["Move"].IsPressed())
            {
                footsteps.Post(gameObject); 
            }
            
        }
        
    }
}
