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
        Vector3 moveValue = speed * Time.deltaTime * Time.timeScale * new Vector3(direction.x, 0, direction.y);
        _character.Move(moveValue);
        if(moveValue == Vector3.zero) { return; }
        Look(moveValue.normalized);
        model.position = transform.position;
        if(interactor == null) { return; }
        interactor.position = transform.position + moveValue.normalized;

    }

    public virtual void Look(Vector3 direction)
    {
        float angle = Vector3.Angle(model.forward, direction);
        model.eulerAngles = new Vector3(model.eulerAngles.x, model.eulerAngles.y + angle, model.eulerAngles.z);
    }
}
