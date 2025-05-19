using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnEntry
    {
        public GameObject enemyPrefab;
        public int spawnAmountPerInterval = 1;
    }

    [Header("Enemy Settings")]
    [SerializeField] private List<EnemySpawnEntry> enemySpawnEntries;

    [Header("Spawning Settings")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int numberOfSpawns = 5;
    [SerializeField] private int maxEnemiesAtOnce = 10000; //Set really high for a "don't care"
    [SerializeField] private bool requireOutOfView = true;

    [Header("Player Reference")]
    [SerializeField] private Camera playerCamera;

    private int spawnsCompleted = 0;
    private int currentEnemyCount = 0;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
        if (playerCamera == null && requireOutOfView)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        currentEnemyCount = CountEnemies();
        if (spawnsCompleted >= numberOfSpawns || currentEnemyCount >= maxEnemiesAtOnce) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            if (!requireOutOfView || !IsVisibleToCamera())
            {
                SpawnEnemies();
                spawnsCompleted++;
            }

            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemies()
    {
        foreach (var entry in enemySpawnEntries)
        {
            for (int i = 0; i < entry.spawnAmountPerInterval; i++)
            {
                if (entry.enemyPrefab != null)
                {
                    Instantiate(entry.enemyPrefab, transform.position, transform.rotation);
                    Debug.Log($"Spawned {entry.enemyPrefab.name} at {transform.position}");
                }
            }
        }
    }

    private int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private bool IsVisibleToCamera()
    {
        if (playerCamera == null) return false;

        Collider col = GetComponent<Collider>();
        if (col == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, col.bounds);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
