using System;
using UnityEngine;

/// <summary>
/// Tracks the distance walked in metres. Distance increases with current speed.
/// Fires OnDistanceChanged every frame the game is running.
/// </summary>
public class Lvl3DistanceManager : MonoBehaviour
{
    [SerializeField] private Lvl3DifficultyManager _difficultyManager;
    [SerializeField] private SO_PlayerDatas _playerDatas;

    public event Action<int> OnDistanceChanged;

    private bool _gameStarted;
    private float _distanceFloat;

    private void OnEnable() => _difficultyManager.OnGameStarted += HandleGameStarted;
    private void OnDisable() => _difficultyManager.OnGameStarted -= HandleGameStarted;

    private void HandleGameStarted()
    {
        _distanceFloat = 0f;
        _playerDatas.Score = 0;
        _gameStarted = true;
        OnDistanceChanged?.Invoke(0);
    }

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        _distanceFloat += _difficultyManager.CurrentSpeed * Time.deltaTime;
        int metres = Mathf.FloorToInt(_distanceFloat);

        if (metres != _playerDatas.Score)
        {
            _playerDatas.Score = metres;
            OnDistanceChanged?.Invoke(metres);
        }
    }
}
