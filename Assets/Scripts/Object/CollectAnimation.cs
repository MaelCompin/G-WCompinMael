using System.Collections;
using UnityEngine;

public class CollectAnimation : MonoBehaviour
{
    private Vector3 _originalScale;

    private void Awake() => _originalScale = transform.localScale;

    /// <summary>
    /// Plays the collect scale animation : shrink, grow, shrink to zero.
    /// Total duration : 0.4 seconds.
    /// </summary>
    public IEnumerator Play()
    {
        yield return ScaleTo(_originalScale * 0.7f, 0.1f);
        yield return ScaleTo(_originalScale * 1.3f, 0.15f);
        yield return ScaleTo(Vector3.zero, 0.15f);
    }

    private IEnumerator ScaleTo(Vector3 target, float duration)
    {
        Vector3 start = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        transform.localScale = target;
    }
}