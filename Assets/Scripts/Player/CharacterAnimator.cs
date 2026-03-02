using System.Collections;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustom _inputManager;
    [SerializeField] private RamCollectedDispatcher _ramCollectedDispatcher;
    [SerializeField] private MalusHitDispatcher _malusHitDispatcher;
    [SerializeField] private HealthManager _healthManager;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _moveSprite;
    [SerializeField] private Sprite _ramSprite;
    [SerializeField] private Sprite _hurtSprite;
    [SerializeField] private Sprite _gameOverSprite;

    [SerializeField] private float _ramSpriteDelay = 0.5f;
    [SerializeField] private float _hurtSpriteDelay = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private const float IdleDelay = 0.7f;
    private float _idleTimer = 0f;
    private bool _isMoving = false;
    private bool _isGameOver = false;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void OnEnable()
    {
        _inputManager.OnMoveLeft += OnMoveLeft;
        _inputManager.OnMoveRight += OnMoveRight;
        _ramCollectedDispatcher.OnRamCollected += OnRamCollected;
        _malusHitDispatcher.OnMalusHit += OnMalusHit;
        _healthManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _inputManager.OnMoveLeft -= OnMoveLeft;
        _inputManager.OnMoveRight -= OnMoveRight;
        _ramCollectedDispatcher.OnRamCollected -= OnRamCollected;
        _malusHitDispatcher.OnMalusHit -= OnMalusHit;
        _healthManager.OnGameOver -= OnGameOver;
    }

    private void Update()
    {
        if (_isGameOver || !_isMoving) return;

        _idleTimer += Time.deltaTime;
        if (_idleTimer >= IdleDelay)
            SetIdle();
    }

    private void OnMoveLeft()
    {
        _spriteRenderer.flipX = true;
        SetMoving();
    }

    private void OnMoveRight()
    {
        _spriteRenderer.flipX = false;
        SetMoving();
    }

    private void OnRamCollected()
    {
        StopAllCoroutines();
        StartCoroutine(TemporarySprite(_ramSprite, _ramSpriteDelay));
    }

    private void OnMalusHit(bool isSeringue)
    {
        if (isSeringue) return;
        StopAllCoroutines();
        StartCoroutine(TemporarySprite(_hurtSprite, _hurtSpriteDelay));
    }

    private void OnGameOver()
    {
        _isGameOver = true;
        StopAllCoroutines();
        _spriteRenderer.sprite = _gameOverSprite;
        _spriteRenderer.flipX = false;
    }

    /// <summary>Shows a temporary sprite then returns to idle.</summary>
    private IEnumerator TemporarySprite(Sprite sprite, float duration)
    {
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.flipX = false;
        _isMoving = false;
        yield return new WaitForSeconds(duration);
        SetIdle();
    }

    private void SetMoving()
    {
        if (_isGameOver) return;
        _spriteRenderer.sprite = _moveSprite;
        _isMoving = true;
        _idleTimer = 0f;
    }

    /// <summary>Switches back to the front-facing idle sprite.</summary>
    private void SetIdle()
    {
        _spriteRenderer.sprite = _idleSprite;
        _spriteRenderer.flipX = false;
        _isMoving = false;
        _idleTimer = 0f;
    }
}
