using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartWindowController : MonoBehaviour
{
    [SerializeField] private DifficultyManagerBase _difficultyManager;
    [SerializeField] private GameObject _window;
    [SerializeField] private Transform _leaderboardContent;
    [SerializeField] private GameObject _entryPrefab;
    [SerializeField] private string _leaderboardKey = "LeaderboardData";

    private void Awake()
    {
        Time.timeScale = 0f;
        _window.SetActive(true);
    }

    private void Start() => PopulateLeaderboard();

    /// <summary>Called by the Easy (green) button.</summary>
    public void OnEasyClick()
    {
        PlayClick();
        StartGame(DifficultyLevel.Easy);
    }

    /// <summary>Called by the Medium (yellow) button.</summary>
    public void OnMediumClick()
    {
        PlayClick();
        StartGame(DifficultyLevel.Medium);
    }

    /// <summary>Called by the Hard (red) button.</summary>
    public void OnHardClick()
    {
        PlayClick();
        StartGame(DifficultyLevel.Hard);
    }

    /// <summary>Returns to the menu without replaying the dialogue.</summary>
    public void OnQuitClick()
    {
        PlayClick();
        DialogueController.HasSeenDialogueThisSession = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void StartGame(DifficultyLevel level)
    {
        _difficultyManager.SetDifficulty(level);
        _difficultyManager.StartGame();
        Time.timeScale = 1f;
        _window.SetActive(false);

        // Start music when the game begins
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayMusic();
    }

    private void PlayClick()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayClick();
    }

    private void PopulateLeaderboard()
    {
        foreach (Transform child in _leaderboardContent)
            Destroy(child.gameObject);

        List<LeaderboardEntry> entries = LeaderboardRepository.GetEntries(_leaderboardKey);

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject row = Instantiate(_entryPrefab, _leaderboardContent);
            row.GetComponent<LeaderboardEntryView>().Init(i + 1, entries[i].playerName, entries[i].score);
        }
    }
}
