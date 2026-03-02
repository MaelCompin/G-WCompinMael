using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputPlayerManagerCustom : MonoBehaviour
{
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        HandleTouchInput();
        HandleMouseInput();
        HandleKeyboardInput();
    }

    private void HandleTouchInput()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                DispatchFromScreenPosition(touch.screenPosition);
        }
    }

    private void HandleMouseInput()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            DispatchFromScreenPosition(Mouse.current.position.ReadValue());
    }

    private void HandleKeyboardInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveRight();
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLeft();
    }

    private void DispatchFromScreenPosition(Vector2 screenPosition)
    {
        if (screenPosition.x < Screen.width / 2f)
            MoveLeft();
        else
            MoveRight();
    }

    /// <summary>Triggers the move left event.</summary>
    public void MoveLeft() => OnMoveLeft?.Invoke();

    /// <summary>Triggers the move right event.</summary>
    public void MoveRight() => OnMoveRight?.Invoke();
}