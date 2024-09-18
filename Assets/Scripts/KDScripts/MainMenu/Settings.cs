using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private Animator optionsAnim;
    [SerializeField] public Button ExitButton;
    public static Settings Instance;
    public Slider slider;
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetSpecificVolume("Master");
            SetSpecificVolume("Music");
            SetSpecificVolume("SFX");
            options.SetActive(false);
        }
    }
    public void SetSpecificVolume(string whatValue)
    {
        if(whatValue == "Master")
        {
            masterVolume = slider.value;
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
        }
        if (whatValue == "Music")
        {
            musicVolume = slider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }
        if (whatValue == "SFX")
        {
            SFXVolume = slider.value;
            AkSoundEngine.SetRTPCValue("SFXVolume", SFXVolume);
        }
    }

    public IEnumerator Popdown()
    {
        optionsAnim.Play("Popdown");
        while(options.transform.localScale.x > 0.1)
        {
            yield return null;
        }
        poppingDown = null;
        options.SetActive(false);
    }
    private Coroutine poppingDown = null;
    public void ExitSettings()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) 
        { 
            options.SetActive(false); 
            return;
        }
        if(options == null || !options.activeSelf) { return; }
        if(poppingDown != null) { return; }
        poppingDown = StartCoroutine(Popdown());
    }
    public void Popup()
    {
        if(options.activeSelf) { return; }
        if(poppingDown != null) { return; }
        options.SetActive(true);
        optionsAnim.Play("Popup");
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += ExitSettingsOnUnload;
    }
    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= ExitSettingsOnUnload;
    }   
    public void ExitSettingsOnUnload(Scene scene)
    {
        ExitSettings();
    }
}
