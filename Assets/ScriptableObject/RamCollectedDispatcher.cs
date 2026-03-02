using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RamCollectedDispatcher", menuName = "Scriptable Objects/RamCollectedDispatcher")]
public class RamCollectedDispatcher : ScriptableObject
{
    public event Action OnRamCollected;
    public event Action OnRamMissed;

    /// <summary>Fires the RAM collected event.</summary>
    public void Dispatch() => OnRamCollected?.Invoke();

    /// <summary>Fires the RAM missed event.</summary>
    public void DispatchMissed() => OnRamMissed?.Invoke();
}