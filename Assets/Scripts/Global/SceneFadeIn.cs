using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Start() => StartCoroutine(FadeIn());

    /// <summary>Fades the black panel from opaque to transparent on scene load.</summary>
    private IEnumerator FadeIn()
    {
        _fadePanel.gameObject.SetActive(true);
        _fadePanel.color = Color.black;

        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / _fadeDuration);
            _fadePanel.color = new Color(0f, 0f, 0f, 1f - t);
            yield return null;
        }

        _fadePanel.color = Color.clear;
        _fadePanel.gameObject.SetActive(false);
    }
}