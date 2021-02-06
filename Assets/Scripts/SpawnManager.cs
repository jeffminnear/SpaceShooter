using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _enemySpawnDelay = 5f;
    private float _enemyDelayMultiplier = 0.8f;
    private float _minEnemySpawnDelay = 1f;
    private float _maxEnemySpawnDelay = 5f;
    [SerializeField]
    private float _averagePowerUpSpawnDelay = 7.0f;
    private float _minXPosition = -9.3f;
    private float _maxXPosition = 9.3f;
    private int _maxEnemies = 15;
    private bool _shouldSpawn = true;

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        while (_shouldSpawn)
        {
            if (EnemyCount() < _maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(_enemySpawnDelay);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        while (_shouldSpawn)
        {
            if (Time.time > _averagePowerUpSpawnDelay)
            {
                SpawnPowerUp();
            }

            yield return new WaitForSeconds(Random.Range(_averagePowerUpSpawnDelay / 2, _averagePowerUpSpawnDelay * 1.5f));
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(_enemy, GetSpawnPoint(), Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;

        if (_enemySpawnDelay > _minEnemySpawnDelay && _enemySpawnDelay < _maxEnemySpawnDelay)
        {
            _enemySpawnDelay = Mathf.Clamp(_enemySpawnDelay * _enemyDelayMultiplier, _minEnemySpawnDelay, _maxEnemySpawnDelay);
        }
    }

    int EnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void SpawnPowerUp()
    {
        int index = 0;
        int check = Random.Range(1, 101);

        if (check > 33 && check <= 55)
        {
            index = 1;
        }
        else if (check > 55 && check <= 80)
        {
            index = 2;
        }
        else if (check > 80)
        {
            index = 3;
        }

        GameObject powerUP = _powerUps[Mathf.Clamp(index, 0, _powerUps.Length - 1)];
        Instantiate(powerUP, GetSpawnPoint(), Quaternion.identity);
    }

    Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(_minXPosition, _maxXPosition), transform.position.y, 0);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        
        if (_powerUps.Length > 0)
        {
            StartCoroutine(SpawnPowerUpRoutine());
        }
    }

    public void StopSpawning()
    {
        _shouldSpawn = false;
    }
}
