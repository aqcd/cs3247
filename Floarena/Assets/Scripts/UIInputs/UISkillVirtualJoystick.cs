using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UISkillVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
    
    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    public Image uiCircle;

    [Header("Settings")]
    public float joystickRange = 50f;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    public bool isButton = false;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;
    public UnityEvent<Vector2> joystickUpEvent;

    void Start()
    {
        SetupHandle();
    }

    private void SetupHandle()
    {
        if(handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        if (!isButton)
        {
            uiCircle.enabled = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isButton)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        
            position = ApplySizeDelta(position);
        
            Vector2 clampedPosition = ClampValuesToMagnitude(position);

            Vector2 outputPosition = ApplyInversionFilter(position);

            OutputPointerEventValue(outputPosition * magnitudeMultiplier);

            if(handleRect)
            {
                UpdateHandleRectPosition(clampedPosition * joystickRange);
            }
        } else {
            OutputPointerEventValue(Vector2.zero);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isButton)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        
            position = ApplySizeDelta(position);
        
            Vector2 clampedPosition = ClampValuesToMagnitude(position);

            Vector2 outputPosition = ApplyInversionFilter(position);

            OutputPointerEventValue(Vector2.zero);
            uiCircle.enabled = false;

            if(handleRect)
            {
                UpdateHandleRectPosition(Vector2.zero);
            }

            joystickUpEvent.Invoke(outputPosition * magnitudeMultiplier);
        } else {
            OutputPointerEventValue(Vector2.zero);
            uiCircle.enabled = false;
            joystickUpEvent.Invoke(Vector2.zero);
        }
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent.Invoke(pointerPosition);
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x/containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y/containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if(invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if(invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
    
}