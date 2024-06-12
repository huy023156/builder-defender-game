using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPositions;
    private Transform enemyPrefab;

    private IObjectPool<Enemy> objectPool;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private int waveNumber;
    [SerializeField] private int nextWaveTimer = 10;
    private float timer;

    private void Awake()
    {
        enemyPrefab = Resources.Load<Transform>("pfEnemy");

        objectPool = new ObjectPool<Enemy>(CreateEnemy, OnGetFromPool, OnReleaseToPool, 
            OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);

        waveNumber = 0;
    }

    #region Objectpool methods
    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.ObjectPool = objectPool;
        return enemy;
    }

    private void OnReleaseToPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(Enemy pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    #endregion

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            int numberOfEnemies = 3 * waveNumber + 5;

            SpawnWave(numberOfEnemies);

            timer = nextWaveTimer;
            waveNumber++;
        }
    }

    private void SpawnWave(int numberOfEnemies) 
    {
        Vector3 position = GetRandomSpawnPosition();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy enemy = objectPool.Get();
            enemy.transform.position = position + UtilClass.GetRandomDir() * 5f;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int random = Random.Range(0, spawnPositions.Length);

        float offset = 5f;

        Vector3 position = spawnPositions[random].position;

        return position;
    }
}
