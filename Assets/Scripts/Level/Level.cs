using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private Transform segmentParent;
    [SerializeField] private float generationStartHeight = 100f;

    private int currentSegmentIndex = 0;
    private List<GameObject> spawnedSegments = new List<GameObject>();
    private float totalLevelHeight = 0;
    private float levelWidth = 0;

    public event System.Action OnLevelGenerationCompleted;

    public float TotalLevelHeight { get { return totalLevelHeight; } }
    public float LevelWidth { get { return levelWidth; } }

    private void Awake()
    {
        Init();
    }

    private async void Init()
    {
        if (levelConfig != null && levelConfig.GetSequence().Count > 0)
        {
            foreach (var item in levelConfig.GetSequence())
            {
                for (int j = 0; j < item.count; j++)
                {
                    await Task.Delay(30);
                    SpawnSegment(item.prefab);
                }
            }

            OnLevelGenerationCompleted?.Invoke();
        }
        else
        {
            Debug.LogWarning("LevelConfig is not set or empty!");
        }
    }

    private void SpawnSegment(GameObject segmentPrefab)
    {
        Vector3 spawnPosition = CalculateSpawnPosition();

        GameObject newSegmentGO = Instantiate(segmentPrefab, spawnPosition, segmentParent.transform.rotation, segmentParent.transform);
        Segment segmentComponent = newSegmentGO.AddComponent<Segment>();
        spawnedSegments.Add(newSegmentGO);

        float segmentHeight = GetSegmentHeight(newSegmentGO);

        float startOffset = (currentSegmentIndex == 0) ? generationStartHeight : 0f;

        DifficultyZonesInfo currentZone = GetDifficultyZoneForHeight(totalLevelHeight);

        if (currentZone != null)
        {
            segmentComponent.Initialize(segmentHeight, this.LevelWidth, levelConfig, newSegmentGO.transform, currentZone.difficultyLevel, currentZone.obstaclesCount, startOffset);
        }
        else
        {
            segmentComponent.Initialize(segmentHeight, this.LevelWidth, levelConfig, newSegmentGO.transform);
            Debug.LogWarning("No difficulty zone found for current height. No obstacles will be spawned.");
        }


        totalLevelHeight += segmentHeight;
        currentSegmentIndex++;
    }

    private DifficultyZonesInfo GetDifficultyZoneForHeight(float height)
    {
        float cumulativeHeight = 0;
        var zones = levelConfig.GetDifficultyZonesList();

        if (zones == null || zones.Count == 0) return null;

        foreach (var zone in zones)
        {
            cumulativeHeight += zone.zoneHeight;
            if (height < cumulativeHeight)
            {
                return zone;
            }
        }
        // Если высота превышает все зоны, используем параметры последней зоны
        return zones.LastOrDefault();
    }

    private Vector3 CalculateSpawnPosition()
    {
        if (spawnedSegments.Count > 0)
        {
            GameObject lastSegment = spawnedSegments[spawnedSegments.Count - 1];
            float lastSegmentHeight = GetSegmentHeight(lastSegment);
            Vector3 lastSegmentPosition = lastSegment.transform.position;
            return lastSegmentPosition + new Vector3(0, lastSegmentHeight, 0);
        }
        else
        {
            return transform.position;
        }
    }

    private float GetSegmentHeight(GameObject segment)
    {
        BoxCollider2D boxCollider = segment.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            float segmentWidth = boxCollider.size.x;
            if (levelWidth < segmentWidth)
            {
                levelWidth = segmentWidth;
            }
            return boxCollider.size.y;
        }
        return 0;
    }

    public LevelConfig GetLevelConfig()
    {
        return levelConfig;
    }
}