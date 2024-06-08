using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    public void OnAfterDeserialize()
    {
        throw new System.NotImplementedException();
    }

    public void OnBeforeSerialize()
    {
        throw new System.NotImplementedException();
    }
}
