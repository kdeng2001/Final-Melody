using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cutscene : MonoBehaviour
{
    public bool isComplete = false;
    public bool isPlaying = false;
    public string sceneName = "";

    public abstract void Play();
    public virtual void Finish() { isComplete = true; isPlaying = false; }
}
