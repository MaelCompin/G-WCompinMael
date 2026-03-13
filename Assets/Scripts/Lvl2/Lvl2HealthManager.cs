using System;
using UnityEngine;

public class Lvl2HealthManager : MonoBehaviour
{
    [SerializeField] private Lvl2PopupManager _popupManager;
    [SerializeField] private GameObject _gameOverPanel;

    public event Action<int> OnLivesChanged;

    private const int MaxLives = 3;
    private int _currentLives;

    private void Start()
    {
        _currentLives = MaxLives;
        _gameOverPanel.SetActive(false);
        OnLivesChanged?.Invoke(_currentLives);
    }

    /// <summary>Called when a popup times out without being completed.</summary>
    public void OnPopupTimedOut()
    {
        _currentLives--;
        OnLivesChanged?.Invoke(_currentLives);

        if (_currentLives <= 0)
            TriggerGameOver();
    }

    /// <summary>Called when the player clicks "Install" — immediate game over.</summary>
    public void TriggerGameOver()
    {
        _popupManager.StopSpawning();
        Time.timeScale = 0f;
        _gameOverPanel.SetActive(true);
    }
}
