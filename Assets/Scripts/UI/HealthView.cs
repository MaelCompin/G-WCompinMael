using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private Image[] _heartImages;

    private void OnEnable() => _healthManager.OnLivesChanged += UpdateDisplay;
    private void OnDisable() => _healthManager.OnLivesChanged -= UpdateDisplay;
    private void Start() => UpdateDisplay(3);

    /// <summary>Shows or hides heart images based on current lives count.</summary>
    private void UpdateDisplay(int lives)
    {
        for (int i = 0; i < _heartImages.Length; i++)
            _heartImages[i].gameObject.SetActive(i < lives);
    }
}