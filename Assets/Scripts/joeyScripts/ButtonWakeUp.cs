using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWakeUp : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        for (int bogus = 0; bogus < buttons.Count; bogus++)
        {
            buttons[bogus].SetActive(true);
        }
    }
}
