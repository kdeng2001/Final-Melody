using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    public int currIndex { get; private set; }
    [SerializeField] private TextAsset[] texts;
    public bool isTalking { get; private set; }
    private void Awake()
    {
        currIndex = 0;
        isTalking = false;
    }
    public override void OnFinishInteract()
    {
        currIndex++;
        Time.timeScale = 1;
        return;
    }

    public override void OnStartInteract()
    {
        // continue dialogue
        if(DialogueManager.Instance.dialogueIsPlaying && isTalking)
        {
            if(DialogueManager.Instance.displayingChoices) { return; }
            isTalking = true;
            DialogueManager.Instance.ContinueStory();
        }
        // finish dialogue
        else if (isTalking && !DialogueManager.Instance.dialogueIsPlaying){
            isTalking = false;
            OnFinishInteract();
        }
        // begin dialogue
        else
        {
            if(currIndex >= texts.Length) { return; }
            DialogueManager.Instance.EnterDialogueMode(texts[currIndex]);
            DialogueManager.Instance.ContinueStory();
            isTalking = true;
        }

    }
}
