using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    [SerializeField] private GameObject smallZombiePrefab;
    [SerializeField] private GameObject normalZombiePrefab;
    [SerializeField] private GameObject bossZombiePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxSpawnAttempts = 30;

    [Header("Spawn Distribution")]
    [SerializeField] private float spawnDistanceMean = 15f;
    [SerializeField] private float spawnDistanceStdDev = 2f;
    [SerializeField] private float minSpawnDistance = 12f;
    [SerializeField] private float maxSpawnDistance = 20f;
    [SerializeField] private float screenMargin = 0.1f;

    [Header("Wave Settings")]
    [SerializeField] private int bossSpawnLevel = 10;

    private Camera mainCamera;
    private float spawnTimer;
    private bool bossSpawned;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (target == null || LevelManager.instance == null)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        int currentLevel = LevelManager.instance.CurrentLevel;

        if (currentLevel >= bossSpawnLevel)
        {
            if (!bossSpawned)
                SpawnBoss();

            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnZombie(currentLevel);
            spawnTimer = 0f;
        }
    }

    private void SpawnZombie(int level)
    {
        GameObject prefab = GetZombiePrefab(level);

        if (prefab == null)
            return;

        if (!TryGetSpawnPosition(out Vector3 position))
            return;

        Instantiate(prefab, position, Quaternion.identity);
    }

    private GameObject GetZombiePrefab(int level)
    {
        float normalChance = Mathf.InverseLerp(2f, 9f, level);
        float randomValue = Random.value;

        if (randomValue < normalChance)
        {
            return normalZombiePrefab != null
                ? normalZombiePrefab
                : smallZombiePrefab;
        }

        return smallZombiePrefab != null
            ? smallZombiePrefab
            : normalZombiePrefab;
    }

    private void SpawnBoss()
    {
        if (bossZombiePrefab == null)
            return;

        if (!TryGetSpawnPosition(out Vector3 position))
            return;

        Instantiate(bossZombiePrefab, position, Quaternion.identity);

        bossSpawned = true;
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            float distance = Mathf.Clamp(
                RandomGaussian(spawnDistanceMean, spawnDistanceStdDev),
                minSpawnDistance,
                maxSpawnDistance
            );

            Vector2 direction = new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );

            Vector2 candidate =
                (Vector2)target.position + direction * distance;

            Vector3 candidatePosition = new Vector3(
                candidate.x,
                candidate.y,
                target.position.z
            );

            Vector3 viewportPosition =
                mainCamera.WorldToViewportPoint(candidatePosition);

            bool outsideScreen =
                viewportPosition.z > 0f &&
                (
                    viewportPosition.x < -screenMargin ||
                    viewportPosition.x > 1f + screenMargin ||
                    viewportPosition.y < -screenMargin ||
                    viewportPosition.y > 1f + screenMargin
                );

            if (outsideScreen)
            {
                spawnPosition = candidatePosition;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }

    private float RandomGaussian(float mean, float stdDev)
    {
        float u1 = 1f - Random.value;
        float u2 = 1f - Random.value;

        float standardNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) *
                               Mathf.Sin(2f * Mathf.PI * u2);

        return mean + standardNormal * stdDev;
    }

    [Header("Spawn Debug")]
    [SerializeField] private bool showSpawnGizmos = true;

    private void OnDrawGizmosSelected()
    {
        if (!showSpawnGizmos || target == null)
            return;

        Vector3 center = target.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, spawnDistanceMean);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, minSpawnDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxSpawnDistance);
    }
}