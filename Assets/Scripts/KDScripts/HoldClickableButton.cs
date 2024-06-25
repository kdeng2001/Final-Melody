using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldClickableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _holdDuration;
    [SerializeField] private float _holdRate;

    public event Action OnClicked;
    public event Action OnHoldClicked;

    private bool _isHoldingButton;
    private float _elapsedTime;

    // press button and start checking for hold
    public void OnPointerDown(PointerEventData eventData) 
    {
        Debug.Log("begin hold");
        ToggleHoldingButton(true);
        //OnClicked?.Invoke();
    } 

    private void ToggleHoldingButton(bool isPointerDown)
    {
        // toggles hold button
        _isHoldingButton = isPointerDown;
        
        // if holding, begin hold behavior
        if (isPointerDown) 
        {
            StartCoroutine(BeginHold());
        }
    }

    private IEnumerator BeginHold()
    {
        
        // wait holdDuration before handling hold behavior
        yield return new WaitForSeconds(_holdDuration);
        Debug.Log("click");
        OnClicked?.Invoke();
        // while holding button
        while(_isHoldingButton) 
        {
            OnHoldClicked?.Invoke();
            // wait before handling hold condition
            yield return new WaitForSeconds(_holdRate);
            Debug.Log("holding");
        }
    }

    // release button and stop hold
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("release hold");
        ToggleHoldingButton(false);
    }
}