using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private bool isBattle = false;
    [SerializeField] public MusicSceneData[] sceneMusicHandling;

    private void Start()
    {
        foreach(MusicSceneData data in sceneMusicHandling)
        {
            if(data.musicLoadType == MusicLoadType.Start)
            {
                AudioManager.Instance.CreateMusic(data.container.id);
                break;
            }   
        }
    }

    public void ManualLoad(string containerID)
    {
        foreach (MusicSceneData data in sceneMusicHandling)
        {
            if (data.container.id == containerID)
            {
                AudioManager.Instance.CreateMusic(data.container.id);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        if(isBattle) { AudioManager.Instance.UnloadCurrentMusic(); }
    }
}
[System.Serializable]
public struct MusicSceneData
{
    [Tooltip("Check AudioManager for music to play.")] 
    [SerializeField] public MusicContainer container;
    [SerializeField] public MusicLoadType musicLoadType;
    [SerializeField] public MusicUnloadType musicUnloadType;
}
