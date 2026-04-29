using UnityEngine;

/// <summary>
/// Scrolls the rope sprite upward to give the illusion that the funambule is walking forward.
/// Speed matches Lvl3DifficultyManager.CurrentSpeed.
/// Uses a seamless texture offset or a repeating set of two rope segments leap-frogged.
/// </summary>
public class Lvl3RopeScroller : MonoBehaviour
{
    [SerializeField] private Lvl3DifficultyManager _difficultyManager;

    // Two rope RectTransforms used in a leap-frog to tile infinitely
    [SerializeField] private RectTransform _ropeSegmentA;
    [SerializeField] private RectTransform _ropeSegmentB;

    // Pixel height of one rope segment
    [SerializeField] private float _segmentHeight = 1080f;

    // Pixels-per-unit ratio used to convert world speed to UI pixels/s
    [SerializeField] private float _scrollSpeedScale = 120f;

    private bool _gameStarted;

    private void OnEnable() => _difficultyManager.OnGameStarted += HandleGameStarted;
    private void OnDisable() => _difficultyManager.OnGameStarted -= HandleGameStarted;

    private void HandleGameStarted() => _gameStarted = true;

    private void Update()
    {
        if (!_gameStarted || Time.timeScale == 0f) return;

        float pixelsPerSecond = _difficultyManager.CurrentSpeed * _scrollSpeedScale;
        float delta = pixelsPerSecond * Time.deltaTime;

        MoveSegment(_ropeSegmentA, delta);
        MoveSegment(_ropeSegmentB, delta);

        // Leap-frog: when a segment scrolls fully below the screen, jump it above the other
        WrapSegment(_ropeSegmentA, _ropeSegmentB);
        WrapSegment(_ropeSegmentB, _ropeSegmentA);
    }

    private void MoveSegment(RectTransform seg, float delta)
    {
        Vector2 pos = seg.anchoredPosition;
        pos.y += delta;
        seg.anchoredPosition = pos;
    }

    private void WrapSegment(RectTransform seg, RectTransform other)
    {
        if (seg.anchoredPosition.y > _segmentHeight)
        {
            // Overlap to eliminate visible gap between segments
            float newY = other.anchoredPosition.y - _segmentHeight + 6f;
            seg.anchoredPosition = new Vector2(seg.anchoredPosition.x, newY);
        }
    }
}
