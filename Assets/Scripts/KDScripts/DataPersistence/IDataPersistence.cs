using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    // only reads data
    void LoadData(GameData data);
    // modifies data
    void SaveData(ref GameData data);
}
