using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class GenericBattleNPC : NPCInteractable
{
    [SerializeField] public bool hasBeenBeaten;
    [SerializeField] public string battleScene;
    private string currentScene;    
    private bool battleDebug = false; 
    public virtual void InitiateBattle()
    {
        if (hasBeenBeaten) { return; }
        currentScene = SceneManager.GetActiveScene().name;
        InitiateTransition(true);
    }
    public virtual void EndBattle(bool result) 
    {
        if(hasBeenBeaten == true) { return; } 
        hasBeenBeaten = result;
        EndTransition(currentScene, true);
    }
    public void EndBattleDebug(CallbackContext context, bool result) 
    {
        if(!battleDebug) { return; }
        EndBattle(result);
        DisableBattleDebug();
    }    
    public virtual void InitiateTransition(bool useDefaultTransition)
    {
        PauseAll();
        if (useDefaultTransition) 
        { 
            // load battle scene = true, pause all
            StartCoroutine(DefaultTransition(true, false)); 
        }
    }
    public virtual void EndTransition(string returnScene, bool useDefaultTransition)
    {
        if (useDefaultTransition) 
        { 
            // load battle scene = false, unpause all
            StartCoroutine(DefaultTransition(false, true)); 
        }
    }
    private IEnumerator DefaultTransition(bool load, bool unpauseAtEnd)
    {
        if(!load) { DisableBattleDebug(); }
        LoadSceneManager.Instance.FadeToScreen(LoadSceneManager.Instance.blackScreen);
        yield return new WaitForSeconds(1.5f);
        AsyncOperation operation = load ? 
            SceneManager.LoadSceneAsync(battleScene, LoadSceneMode.Additive) : 
            SceneManager.UnloadSceneAsync(battleScene);
        
        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        LoadSceneManager.Instance.FadeFromScreen(LoadSceneManager.Instance.blackScreen);
        if(load) { EnableBattleDebug(); }
        if (unpauseAtEnd) { UnpauseAll(); }
    }
    public override void FinishDialogue()
    {
        if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        {
            OnFinishInteract();
            InitiateBattle();
        }
    }
    private void PauseAll()
    {
        Player.Instance.PauseInteractor();
        Player.Instance.PauseMovement();
        InGameMenu.Instance.enabled = false;
    }
    private void UnpauseAll()
    {
        Player.Instance.UnpauseInteractor();
        Player.Instance.UnpauseMovement();
        InGameMenu.Instance.enabled = true;
    }
   
    private void EnableBattleDebug()
    {
        battleDebug = true;
        InputAction loseEndBattle = Player.Instance.playerInput.actions["LoseBattleDebug"];
        InputAction winEndBattle = Player.Instance.playerInput.actions["WinBattleDebug"];
        loseEndBattle.started += context => EndBattleDebug(context, false);
        winEndBattle.started += context => EndBattleDebug(context, true);
        
    }
    private void DisableBattleDebug()
    {
        battleDebug = false;
        InputAction loseEndBattle = Player.Instance.playerInput.actions["LoseBattleDebug"];
        InputAction winEndBattle = Player.Instance.playerInput.actions["WinBattleDebug"];
        loseEndBattle.started -= context => EndBattleDebug(context, false);
        winEndBattle.started -= context => EndBattleDebug(context, true);
    }
}
