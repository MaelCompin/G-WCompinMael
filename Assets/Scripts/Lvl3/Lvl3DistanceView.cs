using TMPro;
using UnityEngine;

/// <summary>Displays the current distance walked in metres.</summary>
public class Lvl3DistanceView : MonoBehaviour
{
    [SerializeField] private Lvl3DistanceManager _distanceManager;
    [SerializeField] private TMP_Text _distanceText;

    private void OnEnable()  => _distanceManager.OnDistanceChanged += UpdateDisplay;
    private void OnDisable() => _distanceManager.OnDistanceChanged -= UpdateDisplay;
    private void Start() => UpdateDisplay(0);

    /// <summary>Updates the displayed metre count.</summary>
    private void UpdateDisplay(int metres) => _distanceText.text = $"{metres} m";
}
