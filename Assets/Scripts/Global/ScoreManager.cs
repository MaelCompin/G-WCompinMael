using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private RamCollectedDispatcher _ramCollectedDispatcher;
    [SerializeField] private SO_PlayerDatas _playerDatas;

    public event Action<int> OnScoreChanged;
    public event Action<int> OnMultiplierChanged;

    private const int BasePoints = 10;
    private const int BaseMultiplier = 1;
    private const int MultiplierIncrement = 1;

    private int _currentMultiplier = BaseMultiplier;

    private void Start() => ResetScore();

    private void OnEnable()
    {
        _ramCollectedDispatcher.OnRamCollected += OnRamCollected;
        _ramCollectedDispatcher.OnRamMissed += OnRamMissed;
    }

    private void OnDisable()
    {
        _ramCollectedDispatcher.OnRamCollected -= OnRamCollected;
        _ramCollectedDispatcher.OnRamMissed -= OnRamMissed;
    }

    /// <summary>Adds score based on current multiplier then increases it by 1.</summary>
    private void OnRamCollected()
    {
        _playerDatas.Score += BasePoints * _currentMultiplier;
        _currentMultiplier += MultiplierIncrement;
        OnScoreChanged?.Invoke(_playerDatas.Score);
        OnMultiplierChanged?.Invoke(_currentMultiplier);
    }

    /// <summary>Resets the multiplier to base value when a RAM is missed.</summary>
    private void OnRamMissed()
    {
        _currentMultiplier = BaseMultiplier;
        OnMultiplierChanged?.Invoke(_currentMultiplier);
    }

    /// <summary>Resets score and multiplier to their initial values.</summary>
    public void ResetScore()
    {
        _playerDatas.Score = 0;
        _currentMultiplier = BaseMultiplier;
        OnScoreChanged?.Invoke(0);
        OnMultiplierChanged?.Invoke(_currentMultiplier);
    }
}