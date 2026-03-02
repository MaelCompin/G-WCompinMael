using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private ObjectMovement[] _fallingLines;
    [SerializeField] private GameObject _ramPrefab;
    [SerializeField] private GameObject[] _malusPrefabs;
    [SerializeField] private int _spawnDelayDuration = 1;

    private int _spawnTimer = 0;

    private void OnEnable() => _timeManager.OnTimePassed += TimeGestion;
    private void OnDisable() => _timeManager.OnTimePassed -= TimeGestion;

    private void TimeGestion()
    {
        _spawnTimer++;
        if (_spawnTimer < _spawnDelayDuration) return;
        _spawnTimer = 0;
        SpawnOnRandomAvailableLine();
    }

    /// <summary>
    /// Spawns a random object on a random available FallLine.
    /// </summary>
    private void SpawnOnRandomAvailableLine()
    {
        int availableCount = 0;
        for (int i = 0; i < _fallingLines.Length; i++)
            if (!_fallingLines[i].IsBusy) availableCount++;

        if (availableCount == 0) return;

        int target = Random.Range(0, availableCount);
        int current = 0;

        for (int i = 0; i < _fallingLines.Length; i++)
        {
            if (!_fallingLines[i].IsBusy)
            {
                if (current == target)
                {
                    _fallingLines[i].Init(Instantiate(GetRandomPrefab()));
                    return;
                }
                current++;
            }
        }
    }

    /// <summary>Returns RAM prefab 50% of the time, a random malus prefab otherwise.</summary>
    private GameObject GetRandomPrefab()
    {
        return Random.value < 0.5f ? _ramPrefab : _malusPrefabs[Random.Range(0, _malusPrefabs.Length)];
    }
}