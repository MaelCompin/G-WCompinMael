using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TMP_Text _scoreText;

    private void OnEnable() => _scoreManager.OnScoreChanged += UpdateScoreDisplay;
    private void OnDisable() => _scoreManager.OnScoreChanged -= UpdateScoreDisplay;
    private void Start() => UpdateScoreDisplay(0);

    /// <summary>Updates the score text.</summary>
    private void UpdateScoreDisplay(int score) => _scoreText.text = $"Score : {score}";
}