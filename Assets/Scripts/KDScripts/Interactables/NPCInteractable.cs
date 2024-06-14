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
        Debug.Log("finish npc dialogue");
        isTalking = false;
        if (currIndex < texts.Length - 1) { currIndex++; }
        Time.timeScale = 1;
        return;
    }

    public override void OnStartInteract()
    {
        //Debug.Log("start interact npc");
        // continue dialogue
        if(isTalking)
        {
            Debug.Log("continue npc dialogue");
            if(DialogueManager.Instance.displayingChoices) { return; }
            isTalking = true;
            DialogueManager.Instance.ContinueStory();
        }
        // start dialogue
        else
        {
            
            if(currIndex >= texts.Length) { return; }
            Debug.Log("begin npc dialogue");
            DialogueManager.Instance.EnterDialogueMode(texts[currIndex]);
            //DialogueManager.Instance.ContinueStory();
            isTalking = true;
        }
        // finish dialogue
        if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        {

            OnFinishInteract();
        }

    }
}
