using UnityEngine;

public class Lvl2InstallVirusPopup : Lvl2PopupBase
{
    private const float TimeLimit = 3f;
    private const int Score = 30;

    private void Awake() => _timeLimit = TimeLimit;

    /// <summary>Called by the "Install" button -- triggers an instant game over.</summary>
    public void OnInstallClick()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayClick();

        TriggerGameOver();
    }

    /// <summary>Called by the "Cancel" button -- successfully resolves the popup.</summary>
    public void OnCancelClick()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayClick();

        Complete(Score);
    }
}
