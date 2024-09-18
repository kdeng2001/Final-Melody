using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button SettingsButton;
    public void OnNewGameClicked()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene("IntroRefactor");
        SceneManager.LoadSceneAsync("KDEssentials", LoadSceneMode.Additive);
    }
    public void OnContinueClicked()
    {
        DataPersistenceManager.Instance.LoadGame();
        if(DataPersistenceManager.Instance.globalGameData == null) { return; }
        SceneManager.LoadSceneAsync("KDEssentials", LoadSceneMode.Additive);
    }

    public void MenuQuit()
    {
        Application.Quit();
    }

    private void Start()
    {
        SettingsButton.onClick.AddListener(OpenSettings);
        Settings.Instance.ExitButton.onClick.AddListener(ExitSettings);

        AudioManager.Instance.UnloadAll();
        MusicHandler handler = FindObjectOfType<MusicHandler>();
        handler.ManualLoad(handler.sceneMusicHandling[0].container.id);
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
