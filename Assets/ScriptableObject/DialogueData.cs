using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public Sprite Character;
    [TextArea(2, 5)]
    public string Text;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] Lines;
}