using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemies : MonoBehaviour
{
    [SerializeField] private GameObject _enemyTemplate;
    [SerializeField] private Transform _spawn;

    private Transform[] _spawnPoints;

    private int _currentPointIndex = -1;

    private float _duration = 2.0f;
    private float _runningTime;

    private void Awake()
    {
        _spawnPoints = new Transform[_spawn.childCount];

        for (int i = 0; i < _spawn.childCount; i++)
        {
            _spawnPoints[i] = _spawn.GetChild(i);
        }

        ShufflePoints();
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(_currentPointIndex != _spawnPoints.Length)
        {
            while (_runningTime <= _duration)
            {
                _runningTime += Time.deltaTime;
                yield return null;
            }

            _runningTime = 0;
            Transform currentSpawnPoint = _spawnPoints[GetNextPointIndex()];
            GameObject enemy = Instantiate(_enemyTemplate, currentSpawnPoint.position, Quaternion.identity);
        }
    }

    private void ShufflePoints()
    {
        for (var i = 0; i <= _spawnPoints.Length - 1; i++)
        {
            int point = Random.Range(0, _spawn.childCount);
            Transform tempPoint = _spawnPoints[point];
            _spawnPoints[point] = _spawnPoints[i];
            _spawnPoints[i] = tempPoint;
        }
    }

    private int GetNextPointIndex()
    {
        _currentPointIndex++;

        if (_currentPointIndex == _spawnPoints.Length)
        {
            ShufflePoints();
            _currentPointIndex = 0;
        }
        return _currentPointIndex;
    }
}