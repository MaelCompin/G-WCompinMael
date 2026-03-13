using System.Collections;
using UnityEngine;

public class Lvl2PopupManager : MonoBehaviour
{
    [SerializeField] private Lvl2DifficultyManager _difficultyManager;
    [SerializeField] private Lvl2HealthManager _healthManager;
    [SerializeField] private Lvl2ScoreManager _scoreManager;
    [SerializeField] private RectTransform _popupContainer;

    [SerializeField] private GameObject _clickPopupPrefab;
    [SerializeField] private GameObject _installVirusPopupPrefab;
    [SerializeField] private GameObject _installVirusInvertedPopupPrefab;
    [SerializeField] private GameObject _systemErrorPopupPrefab;

    [SerializeField] private float _spawnRangeX = 500f;
    [SerializeField] private float _spawnRangeY = 250f;

    private bool _isRunning = false;
    private Coroutine _spawnCoroutine;

    private void Start()
    {
        _difficultyManager.OnGameStarted += StartSpawning;
    }

    private void OnDestroy()
    {
        _difficultyManager.OnGameStarted -= StartSpawning;
    }

    /// <summary>Starts the popup spawn loop.</summary>
    public void StartSpawning()
    {
        _isRunning = true;
        _spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>Stops the popup spawn loop and clears all active popups.</summary>
    public void StopSpawning()
    {
        _isRunning = false;
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        foreach (Transform child in _popupContainer)
            Destroy(child.gameObject);
    }

    private IEnumerator SpawnLoop()
    {
        while (_isRunning)
        {
            yield return new WaitForSeconds(_difficultyManager.SpawnInterval);
            if (_isRunning)
                SpawnRandomPopup();
        }
    }

    private void SpawnRandomPopup()
    {
        // 1/3 ClickPopup, 1/3 SystemError, 1/3 InstallVirus (normal + inversé)
        GameObject[] prefabs = {
            _clickPopupPrefab,
            _clickPopupPrefab,
            _systemErrorPopupPrefab,
            _systemErrorPopupPrefab,
            _installVirusPopupPrefab,
            _installVirusInvertedPopupPrefab
        };

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        if (prefab == null)
        {
            Debug.LogError("[Lvl2PopupManager] Un prefab n'est pas assigné dans l'Inspector.");
            return;
        }

        GameObject popup = Instantiate(prefab, _popupContainer);

        Lvl2PopupBase popupBase = popup.GetComponentInChildren<Lvl2PopupBase>();

        if (popupBase == null)
        {
            Debug.LogError($"[Lvl2PopupManager] Lvl2PopupBase introuvable sur {prefab.name}. Vérifie que le script est bien attaché.");
            Destroy(popup);
            return;
        }

        RectTransform rt = popupBase.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(
            Random.Range(-_spawnRangeX, _spawnRangeX),
            Random.Range(-_spawnRangeY, _spawnRangeY)
        );

        popupBase.OnCompleted += _scoreManager.AddScore;
        popupBase.OnTimedOut += _healthManager.OnPopupTimedOut;
        popupBase.OnInstantGameOver += _healthManager.TriggerGameOver;
    }
}
