using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    public int currIndex { get; protected set; }
    [SerializeField] private TextAsset[] texts;

    [SerializeField] private Sprite upLook;
    [SerializeField] private Sprite downLook;
    [SerializeField] private Sprite leftLook;
    [SerializeField] private Sprite rightLook;
    [SerializeField] private SpriteRenderer currentSprite;

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
        interacting = false;
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
            HandleSpriteLook();
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
        interacting = true;
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

    public void HandleSpriteLook()
    {
        Vector3 direction = transform.position - Player.Instance.model.position;
        // face left or right
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            // face right
            if(direction.x > 0)
            {
                Debug.Log("Face right");
                if(rightLook == null) { return; }
                currentSprite.sprite = rightLook;
            }
            // face left
            else
            {
                Debug.Log("Face left");
                if (leftLook == null) { return; }
                currentSprite.sprite = leftLook;
            }
        }
        // face up or down
        else
        {
            // face up
            if(direction.z < 0)
            {
                Debug.Log("Face up");
                if (upLook == null) { return; }
                currentSprite.sprite = upLook;
            }
            // face down
            else
            {
                Debug.Log("Face down");
                if (downLook == null) { return; }
                currentSprite.sprite = downLook;
            }
        }
    }
}

