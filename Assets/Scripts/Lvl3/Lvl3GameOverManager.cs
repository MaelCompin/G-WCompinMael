using System.Collections;
using UnityEngine;

/// <summary>
/// Listens for the fall event and triggers the game over sequence.
/// Plays the lose sound, stops the music, then shows the panel after a delay.
/// </summary>
public class Lvl3GameOverManager : MonoBehaviour
{
    [SerializeField] private Lvl3BalanceManager _balanceManager;
    [SerializeField] private GameObject _gameOverPanel;

    // Delay in seconds before the game over panel appears (lets fall + drop animation play)
    [SerializeField] private float _delayBeforePanel = 1.5f;

    private void OnEnable()  => _balanceManager.OnFall += HandleFall;
    private void OnDisable() => _balanceManager.OnFall -= HandleFall;

    private void Start() => _gameOverPanel.SetActive(false);

    private void HandleFall()
    {
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayLooseLvl3();
            GameAudioManager.Instance.StopMusic();
        }

        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(_delayBeforePanel);
        Time.timeScale = 0f;
        _gameOverPanel.SetActive(true);
    }
}
