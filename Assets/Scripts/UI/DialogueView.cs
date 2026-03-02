using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private TMP_Text _dialogueText;

    /// <summary>
    /// Displays the given dialogue line in the UI.
    /// </summary>
    public void Show(DialogueLine line)
    {
        gameObject.SetActive(true);
        _characterImage.sprite = line.Character;
        _dialogueText.text = line.Text;
    }

    /// <summary>
    /// Hides the dialogue UI.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}