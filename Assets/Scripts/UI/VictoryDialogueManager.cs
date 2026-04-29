using UnityEngine;

/// <summary>
/// Checks if the player has beaten OTAVIO's scores and triggers
/// the appropriate victory dialogues on the Menu scene.
/// Each dialogue plays only once ever, persisted via PlayerPrefs.
/// After all 3 level dialogues have been seen, the final dialogue plays immediately.
/// </summary>
public class VictoryDialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueController _dialogueController;

    [Header("Victory Dialogues")]
    [SerializeField] private DialogueData _victoryLvl1;
    [SerializeField] private DialogueData _victoryLvl2;
    [SerializeField] private DialogueData _victoryLvl3;
    [SerializeField] private DialogueData _victoryFinal;

    private const string Lvl1SeenKey   = "VictoryDialogue_Lvl1";
    private const string Lvl2SeenKey   = "VictoryDialogue_Lvl2";
    private const string Lvl3SeenKey   = "VictoryDialogue_Lvl3";
    private const string FinalSeenKey  = "VictoryDialogue_Final";

    private const string Lvl1LeaderboardKey = "LeaderboardData";
    private const string Lvl2LeaderboardKey = "LeaderboardData_Lvl2";
    private const string Lvl3LeaderboardKey = "LeaderboardData_Lvl3";

    private const int OtavioScoreLvl1 = 18660;
    private const int OtavioScoreLvl2 = 475;
    private const int OtavioScoreLvl3 = 258;
    private const string OtavioName   = "OTAVIO";

    private int _victoryDialoguesEnqueued;

    private void Start()
    {
        // Wait for the intro dialogue to finish before checking victories
        _dialogueController.OnDialogueComplete += OnAnyDialogueComplete;

        // If no intro dialogue is playing, check victories right away
        if (!_dialogueController.IsDialogueActive())
            CheckVictories();
    }

    private void OnDestroy()
    {
        if (_dialogueController != null)
            _dialogueController.OnDialogueComplete -= OnAnyDialogueComplete;
    }

    /// <summary>After the intro finishes, check and enqueue victory dialogues.</summary>
    private void OnAnyDialogueComplete()
    {
        _dialogueController.OnDialogueComplete -= OnAnyDialogueComplete;
        CheckVictories();
    }

    /// <summary>Checks each level for a player score above OTAVIO's and enqueues unseen dialogues.</summary>
    private void CheckVictories()
    {
        _victoryDialoguesEnqueued = 0;

        TryEnqueueVictory(Lvl1LeaderboardKey, OtavioScoreLvl1, Lvl1SeenKey, _victoryLvl1);
        TryEnqueueVictory(Lvl2LeaderboardKey, OtavioScoreLvl2, Lvl2SeenKey, _victoryLvl2);
        TryEnqueueVictory(Lvl3LeaderboardKey, OtavioScoreLvl3, Lvl3SeenKey, _victoryLvl3);

        if (_victoryDialoguesEnqueued > 0)
        {
            _dialogueController.OnDialogueComplete += OnVictoryDialogueComplete;
        }
    }

    /// <summary>Enqueues a victory dialogue if the player beat the target score and hasn't seen it.</summary>
    private void TryEnqueueVictory(string leaderboardKey, int targetScore, string seenKey, DialogueData dialogue)
    {
        if (PlayerPrefs.GetInt(seenKey, 0) == 1) return;

        if (!LeaderboardRepository.HasEntryAboveScore(targetScore, leaderboardKey, OtavioName))
            return;

        PlayerPrefs.SetInt(seenKey, 1);
        PlayerPrefs.Save();

        _dialogueController.EnqueueDialogue(dialogue);
        _victoryDialoguesEnqueued++;

        Debug.Log($"[VictoryDialogue] Enqueued victory dialogue for {seenKey}");
    }

    /// <summary>After each victory dialogue completes, check if the final should play.</summary>
    private void OnVictoryDialogueComplete()
    {
        bool allSeen = PlayerPrefs.GetInt(Lvl1SeenKey, 0) == 1
                    && PlayerPrefs.GetInt(Lvl2SeenKey, 0) == 1
                    && PlayerPrefs.GetInt(Lvl3SeenKey, 0) == 1;

        bool finalSeen = PlayerPrefs.GetInt(FinalSeenKey, 0) == 1;

        if (allSeen && !finalSeen && !_dialogueController.IsDialogueActive())
        {
            PlayerPrefs.SetInt(FinalSeenKey, 1);
            PlayerPrefs.Save();

            _dialogueController.OnDialogueComplete -= OnVictoryDialogueComplete;
            _dialogueController.EnqueueDialogue(_victoryFinal);

            Debug.Log("[VictoryDialogue] All 3 beaten! Final dialogue enqueued.");
        }
    }
}
