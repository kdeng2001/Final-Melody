using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class GenericBattleNPC : NPCInteractable
{
    [SerializeField] public bool HasBeenBeaten;
    [SerializeField] public string BattleScene;
    private string currentScene;
    public virtual void InitiateBattle()
    {
        if (HasBeenBeaten) { return; }
        currentScene = SceneManager.GetActiveScene().name;
        InitiateTransition(true);
        //SceneManager.LoadSceneAsync(BattleScene, LoadSceneMode.Additive);
    }
    public virtual void EndBattle(bool result) 
    {
        Debug.Log("end battle");
        if(HasBeenBeaten == true) { return; } 
        HasBeenBeaten = result;
        EndTransition(currentScene, true);
    }
    public void EndBattleDebug(CallbackContext context, bool result) 
    {
        if(!battleDebug) { return; }
        EndBattle(result);
        DisableBattleDebug();
    }
    public virtual void EndTransition(string returnScene, bool useDefaultTransition)
    {
        if (useDefaultTransition) 
        { 
            // load battle scene = false, unpause all
            StartCoroutine(DefaultTransition(false, true)); 
        }
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
    private IEnumerator DefaultTransition(bool load, bool unpauseAtEnd)
    {
        if(!load) { DisableBattleDebug(); }
        LoadSceneManager.Instance.FadeToScreen(LoadSceneManager.Instance.blackScreen);
        yield return new WaitForSeconds(1.5f);
        AsyncOperation operation = load ? 
            SceneManager.LoadSceneAsync(BattleScene, LoadSceneMode.Additive) : 
            SceneManager.UnloadSceneAsync(BattleScene);
        
       
        //AsyncOperation operation;
        //if(load) { operation = SceneManager.LoadSceneAsync(BattleScene, LoadSceneMode.Additive); }
        //else { operation = SceneManager.UnloadSceneAsync(BattleScene); }
        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        Debug.Log("finish default transition");
        LoadSceneManager.Instance.FadeFromScreen(LoadSceneManager.Instance.blackScreen);
        if(load) { EnableBattleDebug(); }
        if (unpauseAtEnd) { UnpauseAll(); }
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
        Player.Instance.UnPauseMovement();
        InGameMenu.Instance.enabled = true;
    }


    public override void FinishDialogue()
    {
        if (isTalking && !DialogueManager.Instance.dialogueIsPlaying)
        {
            OnFinishInteract();
            InitiateBattle();
        }
    }

    bool battleDebug = false;
    public void EnableBattleDebug()
    {
        battleDebug = true;
        //GenericBattleNPC npc = FindAnyObjectByType<GenericBattleNPC>();
        InputAction loseEndBattle = Player.Instance.playerInput.actions["LoseBattleDebug"];
        InputAction winEndBattle = Player.Instance.playerInput.actions["WinBattleDebug"];
        loseEndBattle.started += context => EndBattleDebug(context, false);
        winEndBattle.started += context => EndBattleDebug(context, true);
        
    }

    

    private void DisableBattleDebug()
    {
        battleDebug = false;
        //GenericBattleNPC npc = FindAnyObjectByType<GenericBattleNPC>();
        InputAction loseEndBattle = Player.Instance.playerInput.actions["LoseBattleDebug"];
        InputAction winEndBattle = Player.Instance.playerInput.actions["WinBattleDebug"];
        loseEndBattle.started -= context => EndBattleDebug(context, false);
        winEndBattle.started -= context => EndBattleDebug(context, true);
    }
}
