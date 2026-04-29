using System;
using UnityEngine;

/// <summary>
/// Core physics of the tightrope. Manages a balance value in [-1, 1].
/// Positive = leaning right, negative = leaning left.
/// Once leaning to one side, momentum keeps pushing that way (positive feedback).
/// When |balance| reaches 1 the funambule falls and fires OnFall.
/// </summary>
public class Lvl3BalanceManager : MonoBehaviour
{
    [SerializeField] private Lvl3DifficultyManager _difficultyManager;
    [SerializeField] private Lvl3InputHandler _inputHandler;

    /// <summary>How fast the player corrects the balance when holding a side (units/s).</summary>
    [SerializeField] private float _correctionStrength = 2.5f;

    /// <summary>How much the current lean amplifies itself (positive feedback).</summary>
    [SerializeField] private float _momentumFactor = 0.3f;

    /// <summary>Small random nudge intensity to break perfect equilibrium.</summary>
    [SerializeField] private float _perturbationIntensity = 0.15f;

    /// <summary>How often (seconds) a new random nudge is applied.</summary>
    [SerializeField] private float _perturbationInterval = 4f;

    public event Action OnFall;
    public event Action<float> OnBalanceChanged;

    public float Balance { get; private set; }

    private bool _gameStarted;
    private bool _hasFallen;
    private float _perturbationTimer;
    private float _currentNudge;

    private void OnEnable() => _difficultyManager.OnGameStarted += HandleGameStarted;
    private void OnDisable() => _difficultyManager.OnGameStarted -= HandleGameStarted;

    private void HandleGameStarted()
    {
        _gameStarted = true;
        // Start with a small random nudge to break equilibrium
        _currentNudge = (UnityEngine.Random.value > 0.5f) ? 1f : -1f;
    }

    private void Update()
    {
        if (!_gameStarted || _hasFallen || Time.timeScale == 0f) return;

        float dt = Time.deltaTime;
        float driftIntensity = _difficultyManager.DriftIntensity;

        // --- Momentum: once leaning, keep pushing that direction ---
        float momentum = Balance * _momentumFactor * driftIntensity;

        // --- Small random perturbation to prevent standing still at 0 ---
        _perturbationTimer += dt;
        if (_perturbationTimer >= _perturbationInterval)
        {
            _perturbationTimer = 0f;
            // Nudge in a random direction, biased toward the current lean
            float bias = Mathf.Sign(Balance + 0.001f);
            _currentNudge = (UnityEngine.Random.value > 0.3f) ? bias : -bias;
        }

        float perturbation = _currentNudge * _perturbationIntensity * driftIntensity;

        // --- Player correction input ---
        float input = _inputHandler.BalanceCorrectionInput;
        float correction = input * _correctionStrength;

        Balance += (momentum + perturbation + correction) * dt;
        Balance = Mathf.Clamp(Balance, -1f, 1f);

        OnBalanceChanged?.Invoke(Balance);

        if (Mathf.Abs(Balance) >= 1f)
            TriggerFall();
    }

    private void TriggerFall()
    {
        if (_hasFallen) return;
        _hasFallen = true;
        OnFall?.Invoke();
    }
}
