using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MalusHitDispatcher", menuName = "Scriptable Objects/MalusHitDispatcher")]
public class MalusHitDispatcher : ScriptableObject
{
    public event Action<bool> OnMalusHit; // bool = isSeringue

    /// <summary>
    /// Fires the malus hit event. Pass true if the object is a seringue.
    /// </summary>
    public void Dispatch(bool isSeringue) => OnMalusHit?.Invoke(isSeringue);
}