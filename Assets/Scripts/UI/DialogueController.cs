using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] private DialogueInputHandler _inputHandler;
    [SerializeField] private GameObject[] _playButtons;

    public static bool HasSeenDialogueThisSession = false;

    private int _currentIndex = 0;
    private bool _dialogueActive = false;

    private void OnEnable() => _inputHandler.OnAdvance += AdvanceDialogue;
    private void OnDisable() => _inputHandler.OnAdvance -= AdvanceDialogue;

    private void Start()
    {
        _currentIndex = 0;

        if (HasSeenDialogueThisSession)
        {
            _dialogueActive = false;
            _dialogueView.Hide();
            SetButtonsActive(true);
            return;
        }

        _dialogueActive = true;
        SetButtonsActive(false);
        _dialogueView.Show(_dialogueData.Lines[_currentIndex]);
    }

    /// <summary>Moves to the next dialogue line, or ends the sequence.</summary>
    private void AdvanceDialogue()
    {
        if (!_dialogueActive) return;

        _currentIndex++;

        if (_currentIndex < _dialogueData.Lines.Length)
        {
            _dialogueView.Show(_dialogueData.Lines[_currentIndex]);
        }
        else
        {
            _dialogueActive = false;
            HasSeenDialogueThisSession = true;
            _dialogueView.Hide();
            SetButtonsActive(true);
        }
    }

    private void SetButtonsActive(bool active)
    {
        foreach (GameObject button in _playButtons)
            if (button != null) button.SetActive(active);
    }
}