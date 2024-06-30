using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSprite : MonoBehaviour
{
    [SerializeField] private Sprite drummer;
    [SerializeField] private Sprite guitarist;
    [SerializeField] private Sprite keytarist;
    [SerializeField] private SpriteRenderer sprite;

    private void Start()
    {
        if (
            DataPersistenceManager.Instance != null && DataPersistenceManager.Instance.localGameData != null &&
            DataPersistenceManager.Instance.localGameData.itemAmountInventory.ContainsKey("Guitar")
            )
        {
            Debug.Log("guitarist battler!");
            sprite.sprite = guitarist;
        }
        else if (DataPersistenceManager.Instance != null && DataPersistenceManager.Instance.localGameData != null &&
            DataPersistenceManager.Instance.localGameData.itemAmountInventory.ContainsKey("Drums"))
        {
            Debug.Log("drummer battler!");
            sprite.sprite = drummer;
        }
        else
        {
            Debug.Log("drummer keytarist!");
            sprite.sprite = keytarist;
        }
    }
}
