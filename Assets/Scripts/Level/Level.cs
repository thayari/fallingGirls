using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private Transform segmentParent;
    [SerializeField] private float generationStartHeight = 100f;

    [SerializeField] private int activeSegmentsBuffer = 2;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float generationTriggerDistance = 20f;
    [SerializeField] private float deletionDistance = 10f;

    private int currentSegmentIndexInSequence = 0;
    private List<GameObject> activeSegments = new List<GameObject>();
    private float totalGeneratedHeight = 0;

    //private int currentSegmentIndex = 0;
    //private List<GameObject> spawnedSegments = new List<GameObject>();
    //private float totalLevelHeight = 0;
    private float levelWidth = 0;

    public event System.Action OnLevelGenerationCompleted;
    public event System.Action<GameObject> OnSegmentSpawned;

    public float TotalLevelHeight { get { return totalGeneratedHeight; } }
    public float LevelWidth { get { return levelWidth; } }

    private void Awake()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in Level script. Dynamic segment generation will not work correctly.");
            GameObject playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
            {
                playerTransform = playerGO.transform;
            }
        }

        Init();
    }

    private void Update()
    {
        if (playerTransform == null || activeSegments.Count == 0) return;

        GameObject lastActiveSegment = activeSegments.Last();
        float lastSegmentTopY = lastActiveSegment.transform.position.y + GetSegmentHeight(lastActiveSegment);

        if (playerTransform.position.y + generationTriggerDistance >= lastSegmentTopY)
        {
            GenerateNextSegment();
            CleanUpOldSegments();
        }
    }

    private async void Init()
    {
        if (levelConfig != null && levelConfig.GetSequence().Count > 0)
        {
            for (int i = 0; i < activeSegmentsBuffer; i++)
            {
                await Task.Delay(30);
                SpawnSegment(levelConfig.GetSequence()[i].prefab);
                currentSegmentIndexInSequence++;
            }

            OnLevelGenerationCompleted?.Invoke();
        }
        else
        {
            Debug.LogWarning("LevelConfig is not set or empty!");
        }
    }

    private void GenerateNextSegment()
    {
        GameObject segmentPrefab = levelConfig.GetSequence()[currentSegmentIndexInSequence].prefab;
        SpawnSegment(segmentPrefab);
        currentSegmentIndexInSequence++;
    }

    private void SpawnSegment(GameObject segmentPrefab)
    {
        Vector3 spawnPosition = CalculateSpawnPosition();

        GameObject newSegmentGO = Instantiate(segmentPrefab, spawnPosition, segmentParent.transform.rotation, segmentParent.transform);
        Segment segmentComponent = newSegmentGO.AddComponent<Segment>();
        activeSegments.Add(newSegmentGO);

        float segmentHeight = GetSegmentHeight(newSegmentGO);

        float obstaclesStartOffset = (activeSegments.Count == 1 &&  totalGeneratedHeight == 0) ? generationStartHeight : 0f;

        DifficultyZonesInfo currentZone = GetDifficultyZoneForHeight(totalGeneratedHeight);

        if (currentZone != null)
        {
            segmentComponent.Initialize(segmentHeight, this.LevelWidth, levelConfig, newSegmentGO.transform, currentZone.difficultyLevel, currentZone.obstaclesCount, obstaclesStartOffset);
        }
        else
        {
            segmentComponent.Initialize(segmentHeight, this.LevelWidth, levelConfig, newSegmentGO.transform);
            Debug.LogWarning("No difficulty zone found for current height. No obstacles will be spawned.");
        }


        totalGeneratedHeight += segmentHeight;
        OnSegmentSpawned?.Invoke(newSegmentGO);
    }

    private void CleanUpOldSegments()
    {
        if (playerTransform == null || activeSegments.Count <= activeSegmentsBuffer) { return; }

        while (activeSegments.Count > activeSegmentsBuffer) 
        {
            GameObject segmentToRemove = activeSegments[0];

            float segmentHeight = GetSegmentHeight(segmentToRemove);
            float segmentTopY = segmentToRemove.transform.position.y + segmentHeight;

            if (playerTransform.transform.position.y > segmentTopY + deletionDistance) 
            { 
                activeSegments.RemoveAt(0);
                Destroy(segmentToRemove);
            }
            else
            {
                break;
            }
        }
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
        if (activeSegments.Count > 0)
        {
            GameObject lastSegment = activeSegments.Last();
            float lastSegmentHeight = GetSegmentHeight(lastSegment);
            Vector3 lastSegmentPosition = lastSegment.transform.position;
            return lastSegmentPosition + new Vector3(0, lastSegmentHeight, 0);
        }
        else
        {
            // только для первого сегмента, который должен появиться в исходной точке
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