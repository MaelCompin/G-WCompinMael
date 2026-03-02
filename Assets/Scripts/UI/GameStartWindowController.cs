using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartWindowController : MonoBehaviour
{
    [SerializeField] private DifficultyManager _difficultyManager;
    [SerializeField] private GameObject _window;
    [SerializeField] private Transform _leaderboardContent;
    [SerializeField] private GameObject _entryPrefab;

    private void Awake()
    {
        Time.timeScale = 0f;
        _window.SetActive(true);
    }

    private void Start() => PopulateLeaderboard();

    /// <summary>Called by the Easy (green) button — x1.02 every 10s.</summary>
    public void OnEasyClick() => StartGame(DifficultyLevel.Easy);

    /// <summary>Called by the Medium (yellow) button — x1.06 every 10s.</summary>
    public void OnMediumClick() => StartGame(DifficultyLevel.Medium);

    /// <summary>Called by the Hard (red) button — x1.10 every 10s.</summary>
    public void OnHardClick() => StartGame(DifficultyLevel.Hard);

    /// <summary>Returns to the menu without replaying the dialogue.</summary>
    public void OnQuitClick()
    {
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
    }

    private void PopulateLeaderboard()
    {
        foreach (Transform child in _leaderboardContent)
            Destroy(child.gameObject);

        List<LeaderboardEntry> entries = LeaderboardRepository.GetEntries();

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject row = Instantiate(_entryPrefab, _leaderboardContent);
            row.GetComponent<LeaderboardEntryView>().Init(i + 1, entries[i].playerName, entries[i].score);
        }
    }
}