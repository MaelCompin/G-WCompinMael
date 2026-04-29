using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Reads player balance correction input.
/// Mobile: gyroscope tilt (X axis).
/// PC/test: hold left half of screen → correct left, hold right half → correct right.
/// Returns a value in [-1, 1] where -1 = full left correction, +1 = full right correction.
/// </summary>
public class Lvl3InputHandler : MonoBehaviour
{
    [SerializeField] private bool _useGyroscope = true;

    // Sensitivity of the gyroscope tilt (higher = reacts faster to phone tilt)
    [SerializeField] private float _gyroSensitivity = 2f;

    // How far from center (0..1) the dead zone extends — avoids jitter at rest
    [SerializeField] private float _gyroDeadZone = 0.1f;

    public float BalanceCorrectionInput { get; private set; }

    private bool _gyroAvailable;

    private void Update()
    {
        BalanceCorrectionInput = ReadScreenTouch();
    }

    /// <summary>
    /// PC / touch fallback: hold left half of screen → -1, right half → +1.
    /// Uses only the last active touch to prevent multi-touch cancellation.
    /// </summary>
    private float ReadScreenTouch()
    {
        float screenHalfWidth = Screen.width * 0.5f;

        // New Input System touch - use only the last active touch
        if (Touchscreen.current != null)
        {
            float lastValue = 0f;
            bool hasTouch = false;

            var touches = Touchscreen.current.touches;
            for (int i = 0; i < touches.Count; i++)
            {
                TouchControl touch = touches[i];
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.None) continue;

                float x = touch.position.ReadValue().x;
                lastValue = (x < screenHalfWidth) ? -1f : 1f;
                hasTouch = true;
            }

            if (hasTouch) return lastValue;
        }

        // Mouse fallback (left click → side of screen determines direction)
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            float mx = Mouse.current.position.ReadValue().x;
            return (mx < screenHalfWidth) ? -1f : 1f;
        }

        return 0f;
    }
}
