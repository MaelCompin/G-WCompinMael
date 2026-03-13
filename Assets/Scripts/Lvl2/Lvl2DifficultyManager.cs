using System;
using UnityEngine;

public class Lvl2DifficultyManager : DifficultyManagerBase
{
    public event Action OnGameStarted;

    /// <summary>Current spawn interval between popups in seconds.</summary>
    public float SpawnInterval { get; private set; } = 5f;

    private const float FrequencyIncreaseInterval = 10f;
    private const float FrequencyMultiplier = 1.05f;

    private float _timer = 0f;
    private bool _gameStarted = false;

    /// <summary>Sets the base spawn interval based on difficulty.</summary>
    public override void SetDifficulty(DifficultyLevel level)
    {
        SpawnInterval = level switch
        {
            DifficultyLevel.Easy   => 3.5f,
            DifficultyLevel.Medium => 2f,
            DifficultyLevel.Hard   => 1.2f,
            _                      => 3.5f
        };
    }

    /// <summary>Starts popup frequency escalation.</summary>
    public override void StartGame()
    {
        _gameStarted = true;
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        _timer += Time.deltaTime;
        if (_timer < FrequencyIncreaseInterval) return;

        _timer -= FrequencyIncreaseInterval;
        SpawnInterval /= FrequencyMultiplier;
    }
}
