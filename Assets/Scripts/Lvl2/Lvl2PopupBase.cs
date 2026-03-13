using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Abstract base class for all Lvl2 popup windows.</summary>
public abstract class Lvl2PopupBase : MonoBehaviour
{
    [SerializeField] protected Image _timerBar;

    public event Action<int> OnCompleted;
    public event Action OnTimedOut;
    public event Action OnInstantGameOver;

    protected float _timeLimit;
    private float _elapsed;

    protected virtual void Update()
    {
        if (Time.timeScale == 0f) return;

        _elapsed += Time.deltaTime;

        if (_timerBar != null)
            _timerBar.fillAmount = Mathf.Clamp01(1f - (_elapsed / _timeLimit));

        if (_elapsed >= _timeLimit)
            TimedOut();
    }

    /// <summary>Call when the player successfully completes the popup.</summary>
    protected void Complete(int score)
    {
        OnCompleted?.Invoke(score);
        Destroy(gameObject);
    }

    /// <summary>Called automatically when the timer runs out.</summary>
    protected void TimedOut()
    {
        OnTimedOut?.Invoke();
        Destroy(gameObject);
    }

    /// <summary>Call when the player triggers an instant game over (e.g. clicks Install).</summary>
    protected void TriggerGameOver()
    {
        OnInstantGameOver?.Invoke();
        Destroy(gameObject);
    }
}
