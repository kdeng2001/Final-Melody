using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimHandler : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    // directions
    public static string dLeft = "Left";
    public static string dRight = "Right";
    public static string dUp = "Up";
    public static string dDown = "Down";
    // actions
    public static string aIdle = "Idle";
    public static string aWalk = "Walk";
    public static string aRun = "Run";

    private string currentDirection = "Right";
    private string currentAction = "Idle";
    public string currentAnimation = "IdleRight";
    public void PlayAnimation(string action, string direction)
    {
        string newAnimation = action + direction;
        if(currentAnimation == newAnimation) { return; }
        characterAnimator.Play(newAnimation);
        currentAnimation = newAnimation;
        currentDirection = direction;
        currentAction = action;
    }

    // disregards current animation check
    public void SetAnimation(string animation) { characterAnimator.Play(animation); }
    public void Idle()
    {
        currentAnimation = aIdle + currentDirection;
        characterAnimator.Play(currentAnimation);
        currentAction = aIdle;
    }

    public void Speedup()
    {
        characterAnimator.speed = 1.3f;
    }
    public void Slowdown()
    {
        characterAnimator.speed = 0.65f;
    }

}
