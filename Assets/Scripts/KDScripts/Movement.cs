using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] private int speed = 1;
    [SerializeField] private Transform model;
    [SerializeField] private Transform interactor;
    [SerializeField] private int gravity = 10;
    private CharacterController _character;
    public Vector2 direction { get; private set; }
    private void Awake()
    {
        _character = GetComponent<CharacterController>();
    }

    public virtual void Update()
    {
        Move();
    }
    public void SetDirection(CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    public void SetDirection(Vector2 direction) { this.direction = direction; }

    public virtual void Move()
    {
        Vector3 moveValue = speed * Time.deltaTime * Time.timeScale * new Vector3(direction.x, -gravity, direction.y);
        _character.Move(moveValue);

        Vector3 lookDir = new Vector3(moveValue.x, 0, moveValue.z).normalized;        
        if(lookDir == Vector3.zero) { return; }
        Look(lookDir);
        model.position = transform.position;
        if(interactor == null) { return; }
        interactor.position = transform.position + lookDir.normalized;

    }

    public virtual void Look(Vector3 direction)
    {
        float angle = Vector3.Angle(model.forward, direction);
        model.eulerAngles = new Vector3(model.eulerAngles.x, model.eulerAngles.y + angle, model.eulerAngles.z);
    }
}
