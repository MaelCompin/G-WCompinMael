using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _zoomTarget;

    [SerializeField] private float _zoomScale = 5f;
    [SerializeField] private float _zoomDuration = 0.7f;
    [SerializeField] private float _fadeDuration = 0.4f;

    private bool _isTransitioning = false;
    private Vector2 _backgroundInitialPos;

    private void Awake()
    {
        _backgroundInitialPos = _background.anchoredPosition;
    }

    /// <summary>Zooms toward the target point, fades to black, then loads the scene.</summary>
    public void LoadScene(string sceneName)
    {
        if (_isTransitioning) return;
        StartCoroutine(TransitionAndLoad(sceneName));
    }

    private IEnumerator TransitionAndLoad(string sceneName)
    {
        _isTransitioning = true;

        float elapsed = 0f;
        Vector2 zoomOffset = _zoomTarget.anchoredPosition;

        // Phase 1 : zoom vers le centre de l'écran
        while (elapsed < _zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / _zoomDuration);
            float scale = Mathf.Lerp(1f, _zoomScale, t);

            _background.localScale = Vector3.one * scale;

            // Décale le background pour garder le ZoomTarget au centre
            _background.anchoredPosition = _backgroundInitialPos - zoomOffset * (scale - 1f);

            yield return null;
        }

        // Phase 2 : fondu noir
        _fadePanel.gameObject.SetActive(true);
        elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / _fadeDuration);
            _fadePanel.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        _fadePanel.color = Color.black;
        SceneManager.LoadScene(sceneName);
    }
}
