using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnMainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        InGameMenu.Instance.destroyLoadMainMenu += DestroyThisObject;
    }
    private void OnDisable()
    {
        InGameMenu.Instance.destroyLoadMainMenu -= DestroyThisObject;
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
