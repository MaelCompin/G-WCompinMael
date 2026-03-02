using System;
using System.Collections;
using UnityEngine;

public class RamCollectible : MonoBehaviour
{
    public event Action OnCollected;

    [SerializeField] private RamCollectedDispatcher _ramCollectedDispatcher;

    private const string PlayerTag = "Player";

    private bool _wasCollected = false;

    /// <summary>
    /// Triggers the collect animation then fires the collected event.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(PlayerTag)) return;
        StartCoroutine(CollectAfterAnimation());
    }

    private IEnumerator CollectAfterAnimation()
    {
        _wasCollected = true;

        var animation = GetComponent<CollectAnimation>();
        if (animation != null)
            yield return StartCoroutine(animation.Play());

        _ramCollectedDispatcher?.Dispatch();
        OnCollected?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!_wasCollected)
            _ramCollectedDispatcher?.DispatchMissed();
    }
}