using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button SettingsButton;
    public void OnNewGameClicked()
    {
        Debug.Log("new game clicked");
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene("IntroRefactor");
        SceneManager.LoadSceneAsync("KDEssentials", LoadSceneMode.Additive);
        Debug.Log("async adding KDEssentials");
    }
    public void OnContinueClicked()
    {
        Debug.Log("continue clicked");
        DataPersistenceManager.Instance.LoadGame();
        if(DataPersistenceManager.Instance.globalGameData == null) { return; }
        //SceneManager.LoadScene(DataPersistenceManager.Instance.globalGameData.sceneIndex);
        SceneManager.LoadSceneAsync("KDEssentials", LoadSceneMode.Additive);
        //SceneManager.LoadScene(1);
    }

    public void MenuQuit()
    {
        Application.Quit();
    }

    private void Start()
    {
        SettingsButton.onClick.AddListener(OpenSettings);
        Settings.Instance.ExitButton.onClick.AddListener(ExitSettings);
    }

    private void OpenSettings()
    {
        Settings.Instance.Popup();
    }

    private void ExitSettings()
    {
        Settings.Instance.ExitSettings();
        gameObject.SetActive(true);
    }
}
