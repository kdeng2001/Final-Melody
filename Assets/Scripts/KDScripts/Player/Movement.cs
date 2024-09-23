using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] private int speed = 1;

    private float walkSpeed = .7f;
    private float runSpeed = 1.75f;
    private float moveRate = 1f;
    [SerializeField] private Transform model;
    [SerializeField] private Transform interactor;
    [SerializeField] private int gravity = 10;
    [SerializeField] public CharacterAnimHandler animHandler;
    private CharacterController character;
    public Vector2 direction { get; private set; }        
    private bool isRunning = false;
    // Look variables
    private const int Up = 1;
    private const int Down = -1;
    private const int Left = -1;
    private const int Right = 1;
    private const int None = 0;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
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
        Vector3 moveValue = speed * moveRate * Time.deltaTime * Time.timeScale * new Vector3(direction.x, -gravity, direction.y);
        character.Move(moveValue);
        Vector3 lookDir = new Vector3(moveValue.x, 0, moveValue.z).normalized;        
        Look(lookDir);
        if (lookDir == Vector3.zero) { return; }
        model.position = transform.position;
        if(interactor == null) { return; }
        interactor.position = transform.position + lookDir.normalized;
    }
    // used for cutscenes
    public IEnumerator SceneMoveTo(Vector3 movePosition)
    {
        // calculate/set direction to move in
        direction = (movePosition - transform.position).normalized;
        // start position
        Vector3 initPosition = transform.position;
        // calculate distance to travel
        float targetDistance = Vector3.Distance(transform.position, movePosition);
        // move while targetDistance has not been covered
        while (Vector3.Distance(transform.position, initPosition) < targetDistance)
        {
            Move();
            yield return null;
        }
        // final adjustment if not properly positioned
        character.Move(movePosition);
    }
    public virtual void Look(Vector3 direction)
    {
        if(direction == Vector3.zero) { animHandler.Idle(); }
        else
        {
            // left anim --> left / left-down
            if((direction.x == Left && direction.z == None) || (direction.x < 0 && direction.z < 0)) 
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dLeft);
            }
            // up anim --> up / left-up
            else if((direction.x == None && direction.z == Up) || (direction.x < 0 && direction.z > 0)) {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dUp);
            }
            // right anim --> right / right-up
            else if((direction.x == Right && direction.z == None) || (direction.x > 0 && direction.z > 0))
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dRight);
            }
            // down anim --> down / right-down
            else if ((direction.x == None && direction.z == Down) || (direction.x > 0 && direction.z < 0))
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dDown);
            }
        }
    }
    public virtual void ToggleRun(CallbackContext context)
    {
        // stop runnning
        if(isRunning) 
        {
            animHandler.Slowdown();
            moveRate = walkSpeed;
            isRunning = false;
        }
        // start running
        else
        {
            animHandler.Speedup();
            moveRate = runSpeed;
            isRunning = true;
        }
    }
}
