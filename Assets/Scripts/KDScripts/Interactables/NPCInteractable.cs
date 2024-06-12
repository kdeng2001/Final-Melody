using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable/*, IDataPersistence*/
{
    public int currIndex { get; private set; }
    [SerializeField] private TextAsset[] texts;
    private void Awake()
    {
        currIndex = 0;
    }
    public override void OnFinishInteract()
    {
        currIndex++;
        return;
    }

    public override void OnStartInteract()
    {
        return;
    }

    public void LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }
}
