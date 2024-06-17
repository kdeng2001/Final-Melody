using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Introduction : MonoBehaviour
{
    [Header("Screens for transitions")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image whiteScreen;
    [SerializeField] private float fadeSpeed = 0.02f;

    [Header("For handling intro dialogue")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private TextAsset[] introDialogues;

    private string playerName = "";
    private int index = 0;

    public delegate void FinishFadeFrom();
    public delegate void FinishFadeTo();
    public FinishFadeFrom finishFadeFrom;
    public FinishFadeTo finishFadeTo;

    // start with black screen
    // initiate dialogue
    // continue dialogue
    // pause dialogue and prompt player to enter name
    // on enter, resume dialogue
    // continue dialogue
    // display instrument choices
    // pick instrument
    // continue dialogue
    // fade to white
    // remove white screen to show room
    // initiate dialogue with mom
    // move player to exit home/room

    private void Awake()
    {
        blackScreen.gameObject.SetActive(true);        
        whiteScreen.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        //StartCoroutine(FadeFrom(blackScreen));
        //StartCoroutine(FadeTo(blackScreen));

        //inputField.gameObject.SetActive(true);

    }
    private void Start()
    {
        Player.Instance.PauseMovement();
        StartIntroDialogue(index);
        DialogueManager.Instance.ContinueStory();
    }
    private void OnEnable()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if(interactAction != null)
        {
            interactAction.started += ContinueDialogue;
        }
    }
    private void OnDisable()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if (interactAction != null)
        {
            interactAction.started -= ContinueDialogue;
        }
    }

    /// <summary>
    /// moves alpha of screen from 0 to 1
    /// </summary>
    /// <param name="screen"></param>
    /// <returns></returns>
    IEnumerator FadeTo(Image screen)
    {
        screen.gameObject.SetActive(true);
        float alpha = 0f;
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        int iterations = (int) (1f / fadeSpeed);
        for (int i=0; i<iterations; i++) 
        {
            yield return new WaitForSeconds(fadeSpeed);
            alpha += fadeSpeed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        }
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1f);
        finishFadeTo?.Invoke();
    }
    /// <summary>
    /// moves alpha of screen from 1 to 0
    /// </summary>
    /// <param name="screen"></param>
    /// <returns></returns>
    IEnumerator FadeFrom(Image screen)
    {
        screen.gameObject.SetActive(true);
        float alpha = 1f;
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        int iterations = (int)(1f / fadeSpeed);
        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(fadeSpeed);
            alpha -= fadeSpeed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        }
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0f);
        screen.gameObject.SetActive(false);
        finishFadeFrom?.Invoke();
    }

    public void SubmitName()
    {
        if(inputField.text == "") { Debug.Log("Empty namefield is not allowed"); }
        else 
        { 
            Debug.Log("Submit!");
            playerName = inputField.text;
            inputField.gameObject.SetActive(false);
            StartIntroDialogue(++index);
            DialogueManager.Instance.currentStory.variablesState["player_name"] = playerName;
        }
    }

    private void StartIntroDialogue(int index)
    {
        DialogueManager.Instance.EnterDialogueMode(introDialogues[index]);
    }

    private void ContinueDialogue(InputAction.CallbackContext ctx)
    {
        if(!DialogueManager.Instance.currentStory.canContinue)
        {
            DialogueManager.Instance.ExitDialogueMode();
            Player.Instance.PauseMovement();
            if(index == 0) { inputField.gameObject.SetActive(true); }
            if(index == 1) 
            {
                //StartCoroutine(FadeFrom(blackScreen));
                finishFadeTo += ClearScreens;
                StartCoroutine(FadeTo(whiteScreen));
                index++;
            }
            return;
        }
        DialogueManager.Instance.ContinueStory();
    }

    public void ClearScreens()
    {
        blackScreen.gameObject.SetActive(false);
        whiteScreen.StartCoroutine(FadeFrom(whiteScreen));
        finishFadeTo -= ClearScreens;
    }
}
