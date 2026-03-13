using UnityEngine;

public class Lvl2SystemErrorPopup : Lvl2PopupBase
{
    private const float TimeLimit = 2f;
    private const int Score = 10;

    private void Awake() => _timeLimit = TimeLimit;

    /// <summary>Called by the "OK" button.</summary>
    public void OnOkClick() => Complete(Score);
}
