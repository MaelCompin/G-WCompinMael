using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputHandler : MonoBehaviour
{
    public event Action OnAdvance;

    private void Update()
    {
        bool tapped = Touchscreen.current != null
                      && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        bool clicked = Mouse.current != null
                       && Mouse.current.leftButton.wasPressedThisFrame;

        if (tapped || clicked)
            OnAdvance?.Invoke();
    }
}