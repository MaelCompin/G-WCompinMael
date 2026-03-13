using UnityEngine;

public enum DifficultyLevel { Easy, Medium, Hard }

public class DifficultyManager : DifficultyManagerBase
{
    [SerializeField] private float _baseTimeStep = 1f;

    private TimeManager[] _allTimeManagers;
    private const float SpeedIncreaseInterval = 10f;

    private float _speedIncrement = 0.02f;
    private float _timer = 0f;
    private float _currentSpeedMultiplier = 1f;
    private bool _gameStarted = false;

    private void Start()
    {
        _allTimeManagers = FindObjectsByType<TimeManager>(FindObjectsSortMode.None);
    }

    /// <summary>Sets the speed increment per interval based on difficulty.</summary>
    public override void SetDifficulty(DifficultyLevel level)
    {
        _speedIncrement = level switch
        {
            DifficultyLevel.Easy   => 0.02f,
            DifficultyLevel.Medium => 0.06f,
            DifficultyLevel.Hard   => 0.10f,
            _                      => 0.02f
        };
    }

    /// <summary>Starts the difficulty progression. Called by GameStartWindowController.</summary>
    public override void StartGame() => _gameStarted = true;

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        _timer += Time.deltaTime;
        if (_timer < SpeedIncreaseInterval) return;

        _timer -= SpeedIncreaseInterval;
        _currentSpeedMultiplier += _speedIncrement;

        float newTimeStep = _baseTimeStep / _currentSpeedMultiplier;
        foreach (TimeManager tm in _allTimeManagers)
            tm.SetTimeStep(newTimeStep);
    }
}