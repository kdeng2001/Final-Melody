using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class IntroDirector : MonoBehaviour
{
    [SerializeField] private PlayableDirector introTimeline;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PlayerInput playerInput;

    private string playerName = "";
    private string currentDialogueName;
    private void Awake()
    {
        inputField.gameObject.SetActive(false);
        if(!introTimeline.gameObject.activeSelf) { return; }
        Player.Instance.PauseMovement();
    }
    public void StartDialogue(TextAsset asset)
    {
        Player.Instance.PauseMovement();
        currentDialogueName = asset.name;
        //Debug.Log("currentDialogueName: " + currentDialogueName);
        if(currentDialogueName == "IntroMomNarration") 
        {
            Debug.Log("it's mum time!");
            momContinue += HandleMomContinue; 
        }

        DialogueManager.Instance.EnterDialogueMode(asset);
        setNameInInk?.Invoke();
        EnableCutsceneIteract();
        introTimeline.Pause();
    }

    int momCount = 0;
    public delegate void MomFirstContinue();
    public MomFirstContinue momContinue;
    public void HandleMomContinue()
    {
        switch(momCount)
        {
            case 0:
                {
                    momCount++;
                    StartCoroutine(WaitAndResumeDialogue(1.1f));
                    return;
                }
            case 1:
                {
                    StartCoroutine(WaitAndResumeDialogue(1.5f));
                    momContinue -= HandleMomContinue;
                    return;
                }
            
        }
        
        
    }

    private IEnumerator WaitAndResumeDialogue(float waitTime)
    {        
               
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
        introTimeline.Resume(); 
        yield return new WaitForSeconds(waitTime);
        introTimeline.Pause();
        Player.Instance.UnpauseInteractor();
    }
    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        // must make choice before continuing dialogue
        if (DialogueManager.Instance.displayingChoices) { return; }
        // no skipping
        else if (DialogueManager.Instance.displayLineCoroutine != null) { return; }

        if (!DialogueManager.Instance.currentStory.canContinue)
        {
            DialogueManager.Instance.ExitDialogueMode();
            Player.Instance.PauseMovement();
            DisableCutsceneIteract();
            introTimeline.Resume();
            return;
        }
        DialogueManager.Instance.ContinueStory();
        momContinue?.Invoke();
    }

    public void EnableCutsceneIteract()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if (interactAction != null)
        {
            interactAction.started += ContinueDialogue;
        }
    }
    public void DisableCutsceneIteract()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if (interactAction != null)
        {
            interactAction.started -= ContinueDialogue;
        }
    }

    // 
    public void DisplayNameField()
    {
        inputField.gameObject.SetActive(true);
        introTimeline.Pause();
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
    }

    public void SubmitName()
    {
        if (inputField.text == "") { Debug.Log("Empty namefield is not allowed"); }
        else
        {
            Debug.Log("Submit!");
            inputField.gameObject.SetActive(false);
            playerName = inputField.text;
            introTimeline.Resume();
            setNameInInk += SetName;
        }
    }
    public delegate void SetNameInInk();
    public SetNameInInk setNameInInk;
    public void SetName() 
    { 
        DialogueManager.Instance.currentStory.variablesState["player_name"] = playerName;
        setNameInInk -= SetName;
    }

    public void UnpauseAll()
    {
        Player.Instance.UnpauseInteractor();
        Player.Instance.UnPauseMovement();
    }
}
