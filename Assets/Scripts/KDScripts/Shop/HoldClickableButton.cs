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

    // press button and start checking for hold
    public void OnPointerDown(PointerEventData eventData) 
    {
        ToggleHoldingButton(true);
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
        OnClicked?.Invoke();
        yield return new WaitForSeconds(_holdDuration);
        
        // while holding button
        while(_isHoldingButton) 
        {
            OnHoldClicked?.Invoke();
            // wait before handling hold condition
            yield return new WaitForSeconds(_holdRate);
        }
    }

    // release button and stop hold
    public void OnPointerUp(PointerEventData eventData)
    {
        ToggleHoldingButton(false);
    }
}