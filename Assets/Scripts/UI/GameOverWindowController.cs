using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindowController : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas _playerDatas;
    [SerializeField] private TMP_InputField _nameInput;

    private void OnEnable()
    {
        _nameInput.text = string.Empty;
        _nameInput.Select();
        _nameInput.ActivateInputField();
    }

    /// <summary>Saves the score to the leaderboard then reloads the level.</summary>
    public void OnConfirmClick()
    {
        string playerName = _nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        LeaderboardRepository.AddEntry(playerName, _playerDatas.Score);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}