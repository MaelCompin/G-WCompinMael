using System;
using UnityEngine;

public class Lvl3DifficultyManager : DifficultyManagerBase
{
    // Vitesse initiale de marche (unités monde/s pour le défilement visuel de la corde)
    [SerializeField] private float _baseSpeed = 2f;

    private const float SpeedIncreaseInterval = 10f;

    // Coefficients d'accélération selon la difficulté
    private float _speedIncrement;

    public float CurrentSpeed { get; private set; }

    // Intensité du déséquilibre passif (modifiée proportionnellement à la vitesse)
    public float DriftIntensity => CurrentSpeed * 0.2f;

    public event Action OnGameStarted;

    private bool _gameStarted;
    private float _timer;

    /// <summary>Sets the speed growth rate based on chosen difficulty.</summary>
    public override void SetDifficulty(DifficultyLevel level)
    {
        _speedIncrement = level switch
        {
            DifficultyLevel.Easy   => 0.15f,
            DifficultyLevel.Medium => 0.35f,
            DifficultyLevel.Hard   => 0.60f,
            _                      => 0.15f
        };
        CurrentSpeed = _baseSpeed;
    }

    /// <summary>Starts the game loop and difficulty progression.</summary>
    public override void StartGame()
    {
        _gameStarted = true;
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        _timer += Time.deltaTime;
        if (_timer < SpeedIncreaseInterval) return;

        _timer -= SpeedIncreaseInterval;
        CurrentSpeed += _speedIncrement;
    }
}
