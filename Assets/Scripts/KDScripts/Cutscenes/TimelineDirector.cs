using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimelineDirector : MonoBehaviour
{
    [SerializeField] public PlayableDirector timeline;
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public PlayerInput playerInput;
    protected string currentDialogueName;
    public virtual void Awake()
    {
        inputField.gameObject.SetActive(false);
    }
    public virtual void StartDialogue(TextAsset asset)
    {
        Player.Instance.PauseMovement();
        DialogueManager.Instance.EnterDialogueMode(asset);
        EnableCutsceneIteract();
        timeline.Pause();
    }
    public IEnumerator ResumeAndWaitDialogue(float waitTime)
    {
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
        timeline.Resume();
        yield return new WaitForSeconds(waitTime);
        timeline.Pause();
        Player.Instance.UnpauseInteractor();
    }
    public virtual void ContinueDialogue(InputAction.CallbackContext context)
    {
        if (!DialogueManager.Instance.currentStory.canContinue)
        {
            DialogueManager.Instance.ExitDialogueMode();
            Player.Instance.PauseMovement();
            DisableCutsceneIteract();
            timeline.Resume();
            return;
        }
        DialogueManager.Instance.ContinueStory();
    }
    private void EnableCutsceneIteract()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if (interactAction != null)
        {
            interactAction.started += ContinueDialogue;
        }
    }
    private void DisableCutsceneIteract()
    {
        InputAction interactAction = playerInput.actions["Interact"];
        if (interactAction != null)
        {
            interactAction.started -= ContinueDialogue;
        }
    }
    public void DisplayField()
    {
        inputField.gameObject.SetActive(true);
        timeline.Pause();
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
    }
    public void PauseAll()
    {
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
        InGameMenu.Instance.enabled = false;
    }
    public void UnpauseAll()
    {
        Player.Instance.UnpauseInteractor();
        Player.Instance.UnpauseMovement();
        InGameMenu.Instance.enabled = true;
    }
}
