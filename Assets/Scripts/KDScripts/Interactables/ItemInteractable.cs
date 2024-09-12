using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    // item variables
    [SerializeField] Item item;
    public delegate void ItemWasObtained(string name, int amount, string iconPath);
    public ItemWasObtained itemWasObtained;

    // dialogue variables
    public int currIndex { get; private set; }
    [SerializeField] private TextAsset[] texts;
    public bool isTalking { get; private set; }
    private bool obtained = false;
    private void Awake()
    {
        currIndex = 0;
        isTalking = false;
        item = transform.parent.GetComponent<Item>();
    }
    public override void OnFinishInteract()
    {
        interacting = false;
        if(obtained) { return; }
        itemWasObtained?.Invoke(item.itemName, item.amount, item.iconFilePath);
        item.HandleObtained();
        obtained = true;
        itemWasObtained -= InventoryUI.Instance.inventory.UpdateItem;
    }

    public override void OnStartInteract()
    {
        interacting = true;
        // continue dialogue
        if (isTalking)
        {
            if (DialogueManager.Instance.displayingChoices) { return; }
            isTalking = true;
            DialogueManager.Instance.ContinueStory();
        }
        // start dialogue
        else
        {
            if (currIndex >= texts.Length) { return; }
            DialogueManager.Instance.EnterDialogueMode(texts[currIndex]);
            // set up variables in item dialogue;
            DialogueManager.Instance.currentStory.variablesState["item"] = item.itemName;
            DialogueManager.Instance.currentStory.variablesState["amount"] = item.amount.ToString();
            isTalking = true;
            itemWasObtained += InventoryUI.Instance.inventory.UpdateItem;
        }
        // finish dialogue
        if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        {
            OnFinishInteract();
        }
    }
}
