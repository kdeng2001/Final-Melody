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
    [SerializeField] public CharacterAnimHandler animHandler;
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
        
        Look(lookDir);
        if (lookDir == Vector3.zero) { return; }
        model.position = transform.position;
        if(interactor == null) { return; }
        interactor.position = transform.position + lookDir.normalized;

    }
    // used for cutscenes
    public IEnumerator SceneMoveTo(Vector3 movePosition, int speed)
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
        _character.Move(movePosition);
    }

    int Up = 1;
    int Down = -1;
    int Left = 1;
    int Right = -1;
    int None = 0;
    float diagonal = 0.71f;
    public virtual void Look(Vector3 direction)
    {
        //float angle = Vector3.Angle(model.forward, direction);
        //model.eulerAngles = new Vector3(model.eulerAngles.x, model.eulerAngles.y + angle, model.eulerAngles.z);

        if(direction == Vector3.zero) { animHandler.Idle(); }
        else
        {
            Debug.Log("direction: " + direction);
            // left anim --> left / left-down
            if((direction.x == Left && direction.z == None) || (direction.x > 0 && direction.z < 0)) 
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dLeft);
            }
            // up anim --> up / left-up
            else if((direction.x == None && direction.z == Up) || (direction.x > 0 && direction.z > 0)) {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dUp);
            }
            // right anim --> right / right-up
            else if((direction.x == Right && direction.z == None) || (direction.x < 0 && direction.z > 0))
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dRight);
            }
            // down anim --> down / right-down
            else if ((direction.x == None && direction.z == Down) || (direction.x < 0 && direction.z < 0))
            {
                animHandler.PlayAnimation(CharacterAnimHandler.aWalk, CharacterAnimHandler.dDown);
            }
        }
    }
}
