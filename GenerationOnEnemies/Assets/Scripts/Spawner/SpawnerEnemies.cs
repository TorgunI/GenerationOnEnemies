using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(Transform))]

public class SpawnerEnemies : MonoBehaviour
{
    [SerializeField] private GameObject _enemyTemplate;
    [SerializeField] private Transform _spawn;

    private Transform[] _spawnPoints;

    private int _currentPoint = -1;

    private float _duration = 2.0f;
    private float _runningTime;

    private void Start()
    {
        _spawnPoints = new Transform[_spawn.childCount];

        for (int i = 0; i < _spawn.childCount; i++)
        {
            _spawnPoints[i] = _spawn.GetChild(i);
        }

        ShufflePoints();
    }

    private void FixedUpdate()
    {
        StartCoroutine(Spawn(Time.deltaTime));
    }

    private IEnumerator Spawn(float timing)
    {
        _runningTime += timing;

        if (_runningTime >= _duration)
        {
            _runningTime = 0;

            Transform currentSpawnPoint = _spawnPoints[GetPointIndex()];
            GameObject enemy = Instantiate(_enemyTemplate, currentSpawnPoint.position, Quaternion.identity);
        }
        yield return null;
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

    private int GetPointIndex()
    {
        _currentPoint++;

        if (_currentPoint == _spawnPoints.Length)
        {
            ShufflePoints();
            _currentPoint = 0;
        }
        return _currentPoint;
    }
}