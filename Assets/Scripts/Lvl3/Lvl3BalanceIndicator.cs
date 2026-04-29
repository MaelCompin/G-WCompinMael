using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a visual balance bar (UI Image fill) so the player can read the current tilt.
/// Fill direction and color shift from green (balanced) to red (near fall).
/// </summary>
public class Lvl3BalanceIndicator : MonoBehaviour
{
    [SerializeField] private Lvl3BalanceManager _balanceManager;

    // The fill image representing the balance (-1 left, +1 right)
    // Set fill method to Horizontal and origin to Left
    [SerializeField] private Image _fillImage;

    [SerializeField] private Color _safeColor   = Color.green;
    [SerializeField] private Color _dangerColor = Color.red;

    private void OnEnable()  => _balanceManager.OnBalanceChanged += UpdateDisplay;
    private void OnDisable() => _balanceManager.OnBalanceChanged -= UpdateDisplay;

    /// <summary>Maps balance [-1, 1] to fill [0, 1] and color safe→danger.</summary>
    private void UpdateDisplay(float balance)
    {
        // Remap [-1, 1] → [0, 1] for the fill amount
        float fill = (balance + 1f) * 0.5f;
        _fillImage.fillAmount = fill;

        // Color based on how far from center (0 = safe, 1 = danger)
        float danger = Mathf.Abs(balance);
        _fillImage.color = Color.Lerp(_safeColor, _dangerColor, danger);
    }
}
