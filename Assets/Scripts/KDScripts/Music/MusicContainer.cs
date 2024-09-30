using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicContainer : MonoBehaviour
{
    [SerializeField] public AkBank bank;
    [SerializeField] public string id;
    private uint _currentMusicID = uint.MaxValue;
    [SerializeField] private MusicType _type;
    public MusicType type
    {
        get { return _type; }
        private set { }
    }
    [SerializeField] private AK.Wwise.Event _music;
    public AK.Wwise.Event music
    {
        get { return _music; }
        private set { }
    }
    public uint currentMusicID 
    { 
        get { return _currentMusicID; }
        set { }
    }
    public void DestroyContainer() 
    {
        PauseMusic();
        Destroy(gameObject);
    }
    public void PauseMusic()
    {
        if(_currentMusicID == uint.MaxValue) { return; }
        AkSoundEngine.StopPlayingID(_currentMusicID);
        _currentMusicID = uint.MaxValue;
    }
    public void PlayMusic()
    {
        _currentMusicID = music.Post(gameObject);
    }
    public string GetID() { return id; }
}
