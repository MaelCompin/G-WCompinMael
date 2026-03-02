using System.Collections;
using TMPro;
using UnityEngine;

public class MultiplierView : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private float _baseFontSize = 36f;
    [SerializeField] private float _fontSizePerLevel = 3f;

    private const int BaseMultiplier = 1;
    private const float MaxFontSize = 110f;
    private const int MaxColorMultiplier = 30;

    private Vector3 _originalScale;

    private void Awake() => _originalScale = transform.localScale;

    private void OnEnable()  => _scoreManager.OnMultiplierChanged += UpdateDisplay;
    private void OnDisable() => _scoreManager.OnMultiplierChanged -= UpdateDisplay;
    private void Start() => UpdateDisplay(BaseMultiplier);

    /// <summary>Updates multiplier text, color, font size and plays punch animation.</summary>
    private void UpdateDisplay(int multiplier)
    {
        bool isAboveBase = multiplier > BaseMultiplier;
        _multiplierText.enabled = isAboveBase;
        if (!isAboveBase) return;

        float targetSize = _baseFontSize + (multiplier - 2) * _fontSizePerLevel;
        _multiplierText.fontSize = Mathf.Min(targetSize, MaxFontSize);
        _multiplierText.text = $"X{multiplier}";

        Color c = GetMultiplierColor(multiplier);
        _multiplierText.color = c;
        _multiplierText.faceColor = c;
        _multiplierText.enableVertexGradient = false;

        StopAllCoroutines();
        StartCoroutine(PunchScale());
    }

    /// <summary>Black at x2, progressively red up to x30.</summary>
    private Color GetMultiplierColor(int multiplier)
    {
        float t = Mathf.Clamp01((multiplier - 2f) / (MaxColorMultiplier - 2f));
        return Color.Lerp(Color.black, Color.red, t);
    }

    private IEnumerator PunchScale()
    {
        const float halfDuration = 0.1f;
        Vector3 bigScale = _originalScale * 1.35f;

        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(_originalScale, bigScale, elapsed / halfDuration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(bigScale, _originalScale, elapsed / halfDuration);
            yield return null;
        }

        transform.localScale = _originalScale;
    }
}
