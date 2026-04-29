using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] private DialogueInputHandler _inputHandler;
    [SerializeField] private GameObject[] _playButtons;

    private const string IntroSeenKey = "IntroDialogueSeen";

    /// <summary>Kept for backward compat with GameStartWindowController.OnQuitClick.</summary>
    public static bool HasSeenDialogueThisSession = false;

    /// <summary>Raised each time a dialogue sequence finishes.</summary>
    public event Action OnDialogueComplete;

    private DialogueData _activeData;
    private int _currentIndex;
    private bool _dialogueActive;
    private Queue<DialogueData> _queue = new Queue<DialogueData>();

    private void OnEnable() => _inputHandler.OnAdvance += AdvanceDialogue;
    private void OnDisable() => _inputHandler.OnAdvance -= AdvanceDialogue;

    private void Start()
    {
        _currentIndex = 0;

        bool alreadySeen = HasSeenDialogueThisSession || PlayerPrefs.GetInt(IntroSeenKey, 0) == 1;

        if (alreadySeen)
        {
            _dialogueActive = false;
            _dialogueView.Hide();
            SetButtonsActive(true);
            return;
        }

        SetButtonsActive(false);
        StartDialogue(_dialogueData);
    }

    /// <summary>Enqueues a dialogue to play after the current one finishes.</summary>
    public void EnqueueDialogue(DialogueData data)
    {
        if (data == null) return;

        if (!_dialogueActive)
        {
            SetButtonsActive(false);
            StartDialogue(data);
        }
        else
        {
            _queue.Enqueue(data);
        }
    }

    /// <summary>Returns true if a dialogue is currently playing.</summary>
    public bool IsDialogueActive() => _dialogueActive;

    private void StartDialogue(DialogueData data)
    {
        _activeData = data;
        _currentIndex = 0;
        _dialogueActive = true;
        _dialogueView.Show(_activeData.Lines[_currentIndex]);
    }

    /// <summary>Moves to the next dialogue line, or ends the sequence.</summary>
    private void AdvanceDialogue()
    {
        if (!_dialogueActive) return;

        _currentIndex++;

        if (_currentIndex < _activeData.Lines.Length)
        {
            _dialogueView.Show(_activeData.Lines[_currentIndex]);
        }
        else
        {
            _dialogueActive = false;
            HasSeenDialogueThisSession = true;

            // Mark intro as permanently seen
            if (_activeData == _dialogueData)
            {
                PlayerPrefs.SetInt(IntroSeenKey, 1);
                PlayerPrefs.Save();
            }

            OnDialogueComplete?.Invoke();

            // Play next queued dialogue or show buttons
            if (_queue.Count > 0)
            {
                StartDialogue(_queue.Dequeue());
            }
            else
            {
                _dialogueView.Hide();
                SetButtonsActive(true);
            }
        }
    }

    private void SetButtonsActive(bool active)
    {
        foreach (GameObject button in _playButtons)
            if (button != null) button.SetActive(active);
    }
}
