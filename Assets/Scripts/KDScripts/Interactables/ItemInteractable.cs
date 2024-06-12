using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] Item item;
    public delegate void ItemWasObtained(string name, int amount, string iconPath);
    public ItemWasObtained itemWasObtained;

    // dialogue variables
    public int currIndex { get; private set; }
    [SerializeField] private TextAsset[] texts;
    public bool isTalking { get; private set; }

    private void Awake()
    {
        currIndex = 0;
        isTalking = false;
        item = transform.parent.GetComponent<Item>();
    }
    public override void OnFinishInteract()
    {
        currIndex++;  
        itemWasObtained?.Invoke(item.itemName, item.amount, item.iconFilePath);
        item.HandleObtained();
    }

    public override void OnStartInteract()
    {
        //// continue dialogue
        //if (DialogueManager.Instance.dialogueIsPlaying && isTalking )
        //{
        //    if (DialogueManager.Instance.displayingChoices) { return; }
        //    isTalking = true;
        //    DialogueManager.Instance.ContinueStory();
        //}
        // finish dialogue
        //else if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        //{
        //    isTalking = false;
        //    OnFinishInteract();
        //}
        // begin dialogue
        if(DialogueManager.Instance.dialogueIsPlaying && !DialogueManager.Instance.currentStory.canContinue)
        {
            isTalking = false;
            DialogueManager.Instance.ContinueStory();
            OnFinishInteract();
        }
        else
        {
            if (currIndex >= texts.Length) { return; }
            DialogueManager.Instance.EnterDialogueMode(texts[currIndex]);
            // set up variables in item dialogue;
            DialogueManager.Instance.currentStory.variablesState["item"] = item.itemName;
            DialogueManager.Instance.currentStory.variablesState["amount"] = item.amount.ToString();
            
            DialogueManager.Instance.ContinueStory();
            isTalking = true;
        }

    }

    public override void Start()
    {
        base.Start();
        itemWasObtained += InventoryUI.Instance.inventory.UpdateItem;
    }
    private void OnDestroy()
    {
        itemWasObtained -= InventoryUI.Instance.inventory.UpdateItem;
    }
}
