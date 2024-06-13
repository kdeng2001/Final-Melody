using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        Debug.Log("new game clicked");
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadScene(1);
    }
    public void OnContinueClicked()
    {
        Debug.Log("continue clicked");
        DataPersistenceManager.Instance.LoadGame();
        //SceneManager.LoadScene(1);
    }
}
