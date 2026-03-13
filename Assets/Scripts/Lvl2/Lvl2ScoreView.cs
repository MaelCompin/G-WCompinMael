using TMPro;
using UnityEngine;

public class Lvl2ScoreView : MonoBehaviour
{
    [SerializeField] private Lvl2ScoreManager _scoreManager;
    [SerializeField] private TMP_Text _scoreText;

    private void OnEnable() => _scoreManager.OnScoreChanged += UpdateDisplay;
    private void OnDisable() => _scoreManager.OnScoreChanged -= UpdateDisplay;
    private void Start() => UpdateDisplay(0);

    /// <summary>Updates the score text.</summary>
    private void UpdateDisplay(int score) => _scoreText.text = $"Score : {score}";
}
