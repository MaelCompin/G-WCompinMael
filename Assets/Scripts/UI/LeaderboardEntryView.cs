using TMPro;
using UnityEngine;

public class LeaderboardEntryView : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;

    /// <summary>Populates the leaderboard row with rank, name and score.</summary>
    public void Init(int rank, string name, int score)
    {
        _rankText.text  = $"{rank}.";
        _nameText.text  = name;
        _scoreText.text = score.ToString();
    }
}
