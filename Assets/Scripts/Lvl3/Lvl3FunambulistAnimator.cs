using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the funambule visual:
/// - Alternates between two walk sprites at a cadence tied to current speed.
/// - Rotates the character based on the current balance value (lean effect around feet pivot).
/// - Plays a fall rotation then drops the character off screen into the void.
/// </summary>
public class Lvl3FunambulistAnimator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Lvl3BalanceManager _balanceManager;
    [SerializeField] private Lvl3DifficultyManager _difficultyManager;

    [SerializeField] private Sprite _walkSpriteA;
    [SerializeField] private Sprite _walkSpriteB;

    /// <summary>Maximum tilt angle in degrees when balance is at +/-1.</summary>
    [SerializeField] private float _maxTiltAngle = 30f;

    /// <summary>Base step interval in seconds (how fast sprites alternate).</summary>
    [SerializeField] private float _baseStepInterval = 0.4f;

    /// <summary>Fall rotation speed (degrees/s) when the funambule tips over.</summary>
    [SerializeField] private float _fallRotationSpeed = 360f;

    /// <summary>Gravity acceleration (pixels/s^2) for the fall drop.</summary>
    [SerializeField] private float _fallGravity = 2000f;

    /// <summary>Horizontal speed (pixels/s) during the diagonal fall.</summary>
    [SerializeField] private float _fallHorizontalSpeed = 150f;

    private bool _gameStarted;
    private bool _hasFallen;
    private bool _isFallingDown;
    private float _stepTimer;
    private bool _showingA = true;
    private float _fallVelocity;
    private float _fallDirection; // -1 = falling left, +1 = falling right
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _balanceManager.OnBalanceChanged += HandleBalanceChanged;
        _balanceManager.OnFall += HandleFall;
        _difficultyManager.OnGameStarted += HandleGameStarted;
    }

    private void OnDisable()
    {
        _balanceManager.OnBalanceChanged -= HandleBalanceChanged;
        _balanceManager.OnFall -= HandleFall;
        _difficultyManager.OnGameStarted -= HandleGameStarted;
    }

    private void HandleGameStarted()
    {
        _gameStarted = true;
        _image.sprite = _walkSpriteA;
    }

    private void HandleBalanceChanged(float balance)
    {
        if (_hasFallen) return;
        float angle = -balance * _maxTiltAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleFall()
    {
        _hasFallen = true;
    }

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        if (_hasFallen)
        {
            if (!_isFallingDown)
            {
                // Phase 1: tilt toward 90 degrees (lying flat)
                float target = (transform.rotation.eulerAngles.z < 180f) ? 90f : -90f;
                float current = transform.rotation.eulerAngles.z;
                float next = Mathf.MoveTowardsAngle(current, target, _fallRotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, 0f, next);

                float diff = Mathf.Abs(Mathf.DeltaAngle(current, target));
                if (diff < 1f)
                {
                    _isFallingDown = true;
                    _fallVelocity = 0f;
                    // Fall toward the side the character was leaning
                    _fallDirection = (transform.rotation.eulerAngles.z < 180f) ? -1f : 1f;
                }
            }
            else
            {
                // Phase 2: diagonal drop into the void with slow spin
                _fallVelocity += _fallGravity * Time.deltaTime;
                Vector2 pos = _rectTransform.anchoredPosition;
                pos.y -= _fallVelocity * Time.deltaTime;
                pos.x += _fallDirection * _fallHorizontalSpeed * Time.deltaTime;
                _rectTransform.anchoredPosition = pos;

                float currentZ = transform.rotation.eulerAngles.z;
                float spin = _fallDirection * -60f * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, 0f, currentZ + spin);
            }
            return;
        }

        // Walking animation: alternate sprites based on speed
        float stepInterval = _baseStepInterval / Mathf.Max(1f, _difficultyManager.CurrentSpeed * 0.5f);
        _stepTimer += Time.deltaTime;
        if (_stepTimer >= stepInterval)
        {
            _stepTimer = 0f;
            _showingA = !_showingA;
            _image.sprite = _showingA ? _walkSpriteA : _walkSpriteB;
        }
    }
}
