using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _transforms;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    [SerializeField] private AudioType _objectmovement;
    [SerializeField] private AudioType _destruction;

    private int _index = -1;
    private int _lastMovedFrame = -1;
    private GameObject _objectfalling;

    public bool IsBusy => _objectfalling != null;

    /// <summary>
    /// Initializes the falling object and places it at the first position.
    /// Movement is then handled by the TimeManager ticks.
    /// </summary>
    public void Init(GameObject newObject)
    {
        if (_objectfalling != null)
            Destroy(_objectfalling);

        _index = 0;
        _lastMovedFrame = Time.frameCount;
        _objectfalling = newObject;
        _objectfalling.transform.position = _transforms[0].position;
    }

    private void OnEnable() => _timeManager.OnTimePassed += MoveObject;
    private void OnDisable() => _timeManager.OnTimePassed -= MoveObject;

    private void MoveObject()
    {
        if (Time.frameCount == _lastMovedFrame) return;
        _lastMovedFrame = Time.frameCount;

        if (_objectfalling == null)
        {
            _index = -1;
            return;
        }

        _index++;

        if (_index < _transforms.Length)
        {
            _objectfalling.transform.position = _transforms[_index].position;
            _audioEventDispatcher.Playaudio(_objectmovement);
        }
        else
        {
            Destroy(_objectfalling);
            _audioEventDispatcher.Playaudio(_destruction);
            _index = -1;
            _objectfalling = null;
        }
    }
}