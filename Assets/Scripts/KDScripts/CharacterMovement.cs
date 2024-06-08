using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class CharacterMovement : Movement
{
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
}
