using UnityEngine;

/// <summary>
/// Periodically spawns WaterDrop or PollutionDrop prefabs at random X positions
/// above the screen. Spawn interval and pollution chance are exposed to the
/// Inspector so difficulty can be tuned without touching code.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject waterDropPrefab;
    public GameObject pollutionDropPrefab;

    [Header("Spawn Tuning")]
    [Tooltip("Seconds between each spawned drop.")]
    public float spawnInterval = 0.8f;
    [Tooltip("Chance (0-1) that a given spawn is a pollution drop instead of clean water.")]
    [Range(0f, 1f)]
    public float pollutionChance = 0.35f;
    [Tooltip("Y position where new drops appear, above the top of the screen.")]
    public float spawnY = 6f;

    private float timer;
    private float halfWidth;

    private void Start()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            halfWidth = cam.orthographicSize * cam.aspect;
        }
        timer = spawnInterval;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnItem();
            timer = spawnInterval;
        }
    }

    private void SpawnItem()
    {
        bool spawnPollution = Random.value < pollutionChance;
        GameObject prefab = spawnPollution ? pollutionDropPrefab : waterDropPrefab;
        if (prefab == null) return;

        float margin = 0.6f;
        float x = Random.Range(-halfWidth + margin, halfWidth - margin);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
