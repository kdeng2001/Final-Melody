using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private int speed = 1;
    [SerializeField] private Transform model;
    [SerializeField] private Transform interactor;
    private CharacterController _character;
    public Vector2 direction { get; private set; }
    private void Awake()
    {
        _character = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
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
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            InputAction moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed -= context => SetDirection(context);
                moveAction.canceled -= context => SetDirection(context);
            }
        }
    }

    private void Update()
    {
        Move();
    }
    public void SetDirection(CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Move()
    {
        Vector3 moveValue = speed * Time.deltaTime * Time.timeScale * new Vector3(direction.x, 0, direction.y);
        _character.Move(moveValue);
        if(moveValue == Vector3.zero) { return; }
        Look(moveValue.normalized);
        interactor.position = transform.position + moveValue.normalized;
        model.position = transform.position;
    }

    public void Look(Vector3 direction)
    {
        float angle = Vector3.Angle(model.forward, direction);
        model.eulerAngles = new Vector3(model.eulerAngles.x, model.eulerAngles.y + angle, model.eulerAngles.z);
    }
}
