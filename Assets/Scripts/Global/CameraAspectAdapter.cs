using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectAdapter : MonoBehaviour
{
    [SerializeField] private float _referenceOrthographicSize = 5f;
    [SerializeField] private float _referenceWidth = 16f;
    [SerializeField] private float _referenceHeight = 9f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        Adapt();
    }

    /// <summary>
    /// Keeps a fixed world height in landscape.
    /// Wider phones reveal more world space horizontally.
    /// </summary>
    private void Adapt()
    {
        float referenceAspect = _referenceWidth / _referenceHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect >= referenceAspect)
        {
            // Wider than reference → height stays fixed, more width visible
            _camera.orthographicSize = _referenceOrthographicSize;
        }
        else
        {
            // Narrower than reference → adjust to keep full height visible
            _camera.orthographicSize = _referenceOrthographicSize * (referenceAspect / currentAspect);
        }
    }
}