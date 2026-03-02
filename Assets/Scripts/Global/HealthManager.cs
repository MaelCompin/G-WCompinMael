using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private MalusHitDispatcher _malusHitDispatcher;
    [SerializeField] private RamCollectedDispatcher _ramCollectedDispatcher;
    [SerializeField] private GameObject _gameOverPanel;

    public event Action<int> OnLivesChanged;
    public event Action OnGameOver;

    private const int MaxLives = 3;
    private int _currentLives;

    private void Start()
    {
        // Time.timeScale is managed by GameStartWindowController
        _currentLives = MaxLives;
        _gameOverPanel.SetActive(false);
        OnLivesChanged?.Invoke(_currentLives);
    }

    private void OnEnable() => _malusHitDispatcher.OnMalusHit += OnMalusHit;
    private void OnDisable() => _malusHitDispatcher.OnMalusHit -= OnMalusHit;

    /// <summary>
    /// Handles malus hit : seringue gives a life, others remove one and reset multiplier.
    /// </summary>
    private void OnMalusHit(bool isSeringue)
    {
        if (isSeringue)
        {
            _currentLives = Mathf.Min(_currentLives + 1, MaxLives);
        }
        else
        {
            _currentLives--;
            _ramCollectedDispatcher.DispatchMissed();
        }

        OnLivesChanged?.Invoke(_currentLives);

        if (_currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Stops the game and shows the game over panel.
    /// </summary>
    private void TriggerGameOver()
    {
        Time.timeScale = 0f;
        _gameOverPanel.SetActive(true);
        OnGameOver?.Invoke();
    }
}