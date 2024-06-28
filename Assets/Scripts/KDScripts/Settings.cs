using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private Animator optionsAnim;
    public static Settings Instance;
    public Slider slider;
    public float volume = 1;
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
            options.SetActive(false);
        }
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

    public void SetVolume()
    {
        volume = slider.value;
        AkSoundEngine.SetRTPCValue(null, volume);
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

    public void Popup()
    {
        if(options.activeSelf) { /*Debug.Log("options is active");*/ return; }
        if(poppingDown != null) { /*Debug.Log("poppingDown is not null");*/ return; }
        options.SetActive(true);
        optionsAnim.Play("Popup");
        //Debug.Log("Popup!!!");
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
