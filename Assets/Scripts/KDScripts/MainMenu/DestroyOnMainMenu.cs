using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnMainMenu : MonoBehaviour
{
    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
