using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class IntroDirector : TimelineDirector
{
    private string playerName = "";
    private string currentDialogueName;

    public override void StartDialogue(TextAsset asset)
    {
        Player.Instance.PauseMovement();
        currentDialogueName = asset.name;
        //Debug.Log("currentDialogueName: " + currentDialogueName);
        if(currentDialogueName == "IntroMomNarration") 
        {
            Debug.Log("it's mum time!");
            momContinue += HandleMomContinue; 
        }
        base.StartDialogue(asset);
        setNameInInk?.Invoke();
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
                    StartCoroutine(ResumeAndWaitDialogue(1.1f));
                    return;
                }
            case 1:
                {
                    StartCoroutine(ResumeAndWaitDialogue(1.5f));
                    momContinue -= HandleMomContinue;
                    momCount++;
                    return;
                }

        }
    }

    public override void ContinueDialogue(InputAction.CallbackContext context)
    {
        // must make choice before continuing dialogue
        if (DialogueManager.Instance.displayingChoices) { return; }
        // no skipping
        else if (DialogueManager.Instance.displayLineCoroutine != null) { return; }
        base.ContinueDialogue(context);
        momContinue?.Invoke();
    }

    public void SubmitName()
    {
        if (inputField.text == "") { Debug.Log("Empty namefield is not allowed"); }
        else
        {
            Debug.Log("Submit!");
            inputField.gameObject.SetActive(false);
            playerName = inputField.text;
            timeline.Resume();
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
}
