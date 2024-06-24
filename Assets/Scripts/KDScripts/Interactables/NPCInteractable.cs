using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    public int currIndex { get; protected set; }
    [SerializeField] private TextAsset[] texts;
    [HideInInspector]
    public bool isTalking { get; protected set; }
    private void Awake()
    {
        currIndex = 0;
        isTalking = false;
    }
    public override void OnFinishInteract()
    {
        //Debug.Log("finish npc dialogue");
        isTalking = false;
        if (currIndex < texts.Length - 1) { currIndex++; }
        Time.timeScale = 1;
        return;
    }

    public override void OnStartInteract()
    {
        //Debug.Log("start interact npc");
        // continue dialogue
        // start dialogue
        if(isTalking) { ContinueDialogue(); }
        else
        {
            StartDialogue();
        }
        // check finish dialogue
        FinishDialogue();

    }

    public virtual void ContinueDialogue()
    {
        //Debug.Log("continue npc dialogue");
        if (DialogueManager.Instance.displayingChoices) { return; }
        isTalking = true;
        DialogueManager.Instance.ContinueStory();
    }

    public virtual void StartDialogue()
    {
        if (currIndex >= texts.Length) { return; }
        //Debug.Log("begin npc dialogue");
        DialogueManager.Instance.EnterDialogueMode(texts[currIndex]);
        //DialogueManager.Instance.ContinueStory();
        isTalking = true;
    }

    public virtual void FinishDialogue()
    {
        if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        {
            OnFinishInteract();
        }
    }
}
