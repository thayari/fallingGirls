using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private Transform segmentParent;
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

        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, segmentParent.transform.rotation, segmentParent.transform);
        Segment segmentComponent = newSegment.AddComponent<Segment>();

        spawnedSegments.Add(newSegment);

        float segmentHeight = GetSegmentHeight(newSegment);
        segmentComponent.Initialize(segmentHeight, this.LevelWidth, levelConfig);

        totalLevelHeight += segmentHeight;
        currentSegmentIndex++;
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

            float segmentHeight = boxCollider.size.y;
            
            return segmentHeight;
        }
        else
        {
            return 0;
        }
    }


    public LevelConfig GetLevelConfig()
    {
        return levelConfig;
    }
}
