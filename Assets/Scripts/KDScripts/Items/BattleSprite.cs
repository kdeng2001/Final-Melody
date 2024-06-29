using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSprite : MonoBehaviour
{
    [SerializeField] private Sprite drummer;
    [SerializeField] private Sprite guitarist;
    [SerializeField] private Sprite keytarist;
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
        if (
            DataPersistenceManager.Instance != null && DataPersistenceManager.Instance.globalGameData != null &&
            DataPersistenceManager.Instance.globalGameData.itemAmountInventory.ContainsKey("Guitar")
            )
        {
            sprite.sprite = guitarist;
        }
        else if (DataPersistenceManager.Instance != null && DataPersistenceManager.Instance.globalGameData != null &&
            DataPersistenceManager.Instance.globalGameData.itemAmountInventory.ContainsKey("Drums"))
        {
            sprite.sprite = drummer;
        }
        else
        {
            sprite.sprite = keytarist;
        }
    }
}
