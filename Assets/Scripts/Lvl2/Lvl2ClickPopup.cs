using UnityEngine;

public class Lvl2ClickPopup : Lvl2PopupBase
{
    [SerializeField] private RectTransform _progressBar;

    private const float TimeLimit = 4f;
    private const int ClicksRequired = 6;
    private const int Score = 20;

    private int _clickCount;

    private void Awake()
    {
        _timeLimit = TimeLimit;
        if (_progressBar != null)
            _progressBar.localScale = new Vector3(0f, 1f, 1f);
    }

    /// <summary>Called by the clickable button on the popup.</summary>
    public void OnClick()
    {
        _clickCount++;

        // Play escalating click sounds for clicks 1-5
        if (_clickCount <= 5 && GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayClickEscalating(_clickCount);

        if (_progressBar != null)
        {
            float progress = Mathf.Clamp01((float)_clickCount / ClicksRequired);
            _progressBar.localScale = new Vector3(progress, 1f, 1f);
        }

        if (_clickCount >= ClicksRequired)
            Complete(Score);
    }
}
