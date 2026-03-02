using System.Collections;
using UnityEngine;

public class MalusCollectible : MonoBehaviour
{
    [SerializeField] private MalusHitDispatcher _malusHitDispatcher;
    [SerializeField] private bool _isSeringue = false;

    private const string PlayerTag = "Player";
    private const float CollectDelay = 0.3f;

    private bool _wasCollected = false;

    /// <summary>
    /// Triggers collection with a short delay when the player touches the object.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(PlayerTag)) return;
        StartCoroutine(CollectAfterDelay());
    }

    private IEnumerator CollectAfterDelay()
    {
        _wasCollected = true;
        yield return new WaitForSeconds(CollectDelay);
        _malusHitDispatcher.Dispatch(_isSeringue);
        Destroy(gameObject);
    }
}