using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Spawns cloud images randomly inside its RectTransform at each run.
/// Auto-loads sub-sprites from the sprite sheet in the Editor.
/// Ensures a minimum distance between all clouds so they never overlap.
/// </summary>
public class CloudSpawner : MonoBehaviour
{
    private const string SpriteSheetPath = "Assets/Sprite/Lvl3/cloud.png";

    [Header("Cloud Sprites (auto-filled)")]
    [SerializeField] private Sprite[] _sprites;

    [Header("Filter")]
    [Tooltip("Names of the sub-sprites to use.")]
    [SerializeField] private string[] _spriteNames = { "cloud_1", "cloud_2", "cloud_3", "cloud_4", "cloud_12" };

    [Header("Spawn Settings")]
    [SerializeField] private int _cloudCount = 8;
    [SerializeField] private float _minDistance = 250f;
    [SerializeField] private float _cloudScale = 1f;

    [Header("Placement")]
    [SerializeField] private float _horizontalPadding = 100f;
    [SerializeField] private float _verticalPadding = 100f;

    private const int MaxPlacementAttempts = 100;

    private RectTransform _rect;
    private List<Sprite> _filteredSprites;

#if UNITY_EDITOR
    /// <summary>Auto-loads sprites from the sprite sheet when the component is added or modified.</summary>
    private void OnValidate()
    {
        LoadSpritesFromAssetDatabase();
    }

    /// <summary>Auto-loads sprites from the sprite sheet when the component is reset.</summary>
    private void Reset()
    {
        LoadSpritesFromAssetDatabase();
    }

    private void LoadSpritesFromAssetDatabase()
    {
        Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(SpriteSheetPath);
        if (allAssets == null || allAssets.Length == 0) return;

        List<Sprite> found = new List<Sprite>();
        HashSet<string> names = new HashSet<string>(_spriteNames);

        foreach (Object asset in allAssets)
        {
            if (asset is Sprite sprite && names.Contains(sprite.name))
                found.Add(sprite);
        }

        _sprites = found.ToArray();
    }
#endif

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        BuildFilteredList();
        SpawnClouds();
    }

    /// <summary>Builds the runtime sprite list from serialized data.</summary>
    private void BuildFilteredList()
    {
        _filteredSprites = new List<Sprite>();

        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogWarning("[CloudSpawner] No cloud sprites loaded.");
            return;
        }

        foreach (Sprite s in _sprites)
        {
            if (s != null)
                _filteredSprites.Add(s);
        }
    }

    /// <summary>Spawns clouds with random positions, sprites, scales and flips.</summary>
    private void SpawnClouds()
    {
        if (_filteredSprites.Count == 0) return;

        float halfW = _rect.rect.width  * 0.5f - _horizontalPadding;
        float halfH = _rect.rect.height * 0.5f - _verticalPadding;

        List<Vector2> placedPositions = new List<Vector2>();

        for (int i = 0; i < _cloudCount; i++)
        {
            if (!TryGetValidPosition(halfW, halfH, placedPositions, out Vector2 pos))
                continue;

            placedPositions.Add(pos);
            CreateCloudImage(pos, i);
        }
    }

    /// <summary>Attempts to find a position that respects the minimum distance.</summary>
    private bool TryGetValidPosition(float halfW, float halfH, List<Vector2> existing, out Vector2 result)
    {
        for (int attempt = 0; attempt < MaxPlacementAttempts; attempt++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(-halfW, halfW),
                Random.Range(-halfH, halfH)
            );

            if (IsPositionValid(candidate, existing))
            {
                result = candidate;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    /// <summary>Checks if a candidate is far enough from all existing positions.</summary>
    private bool IsPositionValid(Vector2 candidate, List<Vector2> existing)
    {
        for (int i = 0; i < existing.Count; i++)
        {
            if (Vector2.Distance(candidate, existing[i]) < _minDistance)
                return false;
        }
        return true;
    }

    /// <summary>Creates a cloud Image GameObject at the given anchored position.</summary>
    private void CreateCloudImage(Vector2 position, int index)
    {
        GameObject cloudGO = new GameObject($"Cloud_{index}");
        cloudGO.transform.SetParent(transform, false);

        RectTransform rt = cloudGO.AddComponent<RectTransform>();
        rt.anchoredPosition = position;

        Sprite sprite = _filteredSprites[Random.Range(0, _filteredSprites.Count)];
        rt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * _cloudScale;

        // Random horizontal flip
        float flipX = Random.value > 0.5f ? -1f : 1f;
        rt.localScale = new Vector3(flipX, 1f, 1f);

        Image img = cloudGO.AddComponent<Image>();
        img.sprite = sprite;
        img.raycastTarget = false;
        img.preserveAspect = true;

        // Slight random transparency for depth variation
        float alpha = Random.Range(0.5f, 0.9f);
        img.color = new Color(1f, 1f, 1f, alpha);
    }
}
