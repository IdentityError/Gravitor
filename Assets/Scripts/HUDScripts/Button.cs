﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public bool colorPressEffect;
    public Color32 pressedColor;
    public bool resizeOnPress;
    public Vector2 pressedScaleMultiplier;
    public bool playSound = true;
    public string soundTag = "ButtonClick";

    protected Color32 defaultColor;
    protected Image sprite;

    protected void Start()
    {
        sprite = GetComponent<Image>();
        defaultColor = sprite.color;
        EventTrigger trigger = GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => Pressed());

        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => Released());

        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    private void Pressed()
    {
        if (colorPressEffect)
        {
            sprite.color = pressedColor;
        }
        if (resizeOnPress)
        {
            Vector3 newScale = new Vector3(sprite.rectTransform.localScale.x * pressedScaleMultiplier.x, sprite.rectTransform.localScale.y * pressedScaleMultiplier.y, sprite.rectTransform.localScale.z * pressedScaleMultiplier.x);
            sprite.rectTransform.localScale = newScale;
        }
    }

    private void Released()
    {
        if (colorPressEffect)
        { 
            sprite.color = defaultColor;
        }
        if(resizeOnPress)
        {
            sprite.rectTransform.localScale = Vector3.one;
        }
        if(playSound)
        {
            AudioManager.GetInstance().PlaySound("ButtonClick");
        }
    }
}
