using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _timeStepDuration = 1.0f;

    public event Action OnTimePassed;

    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer < _timeStepDuration) return;

        _timer -= _timeStepDuration;
        OnTimePassed?.Invoke();
    }

    /// <summary>Updates the time step duration. Takes effect on the next tick.</summary>
    public void SetTimeStep(float newDuration)
    {
        _timeStepDuration = Mathf.Max(0.1f, newDuration);
    }
}