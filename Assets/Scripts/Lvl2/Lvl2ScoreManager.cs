using System;
using UnityEngine;

public class Lvl2ScoreManager : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas _playerDatas;

    public event Action<int> OnScoreChanged;

    private void Start()
    {
        _playerDatas.Score = 0;
        OnScoreChanged?.Invoke(0);
    }

    /// <summary>Adds score points when a popup is successfully completed.</summary>
    public void AddScore(int points)
    {
        _playerDatas.Score += points;
        OnScoreChanged?.Invoke(_playerDatas.Score);
    }
}
