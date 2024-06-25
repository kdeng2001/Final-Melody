using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 0.02f;
    [SerializeField] public Image blackScreen;
    [SerializeField] public Image whiteScreen;

    public static LoadSceneManager Instance;
    private Coroutine currentFade;

    public delegate void FinishFadeFrom();
    public delegate void FinishFadeTo();
    public FinishFadeFrom finishFadeFrom;
    public FinishFadeTo finishFadeTo;

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
        }
    }
    public void FadeToScreen(Image screen)
    {
        EndCoroutine();
        currentFade = StartCoroutine(FadeTo(screen));
    }
    public void FadeFromScreen(Image screen)
    {
        EndCoroutine();
        currentFade = StartCoroutine(FadeFrom(screen));
    }

    private IEnumerator FadeTo(Image screen)
    {
        screen.gameObject.SetActive(true);
        float alpha = 0f;
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        int iterations = (int)(1f / fadeSpeed);
        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(fadeSpeed);
            alpha += fadeSpeed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        }
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1f);
        finishFadeTo?.Invoke();
        currentFade = null;
    }
    /// <summary>
    /// moves alpha of screen from 1 to 0
    /// </summary>
    /// <param name="screen"></param>
    /// <returns></returns>
    private IEnumerator FadeFrom(Image screen)
    {
        screen.gameObject.SetActive(true);
        float alpha = 1f;
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        int iterations = (int)(1f / fadeSpeed);
        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(fadeSpeed);
            alpha -= fadeSpeed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
        }
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0f);
        screen.gameObject.SetActive(false);
        finishFadeFrom?.Invoke();
        currentFade = null;
    }
    private void EndCoroutine()
    {
        if(currentFade != null) 
        {
            StopCoroutine(currentFade);
            currentFade = null;
        }
    }
}
