using UnityEngine;

/// <summary>Abstract base for difficulty managers shared by all game levels.</summary>
public abstract class DifficultyManagerBase : MonoBehaviour
{
    /// <summary>Sets the difficulty level before the game starts.</summary>
    public abstract void SetDifficulty(DifficultyLevel level);

    /// <summary>Starts the difficulty progression.</summary>
    public abstract void StartGame();
}
