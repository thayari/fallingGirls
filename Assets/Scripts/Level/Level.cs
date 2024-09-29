using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SegmentInfo
{
    public GameObject prefab;
    public int count;
}


public class Level : MonoBehaviour
{
    [SerializeField] private List<SegmentInfo> sequence;
    [SerializeField] private Transform segmentParent;
    private int currentSegmentIndex = 0;
    private List<GameObject> spawnedSegments = new List<GameObject>();
    private float totalLevelHeight = 0;

    public event System.Action OnLevelGenerationCompleted;

    public float TotalLevelHeight
    {
        get { return totalLevelHeight; }
    }

    private void Awake()
    {
        Init();
    }

    private async void Init()
    {
        if (sequence.Count > 0)
        {
            foreach (var item in sequence)
            {
                for (int j = 0; j < item.count; j++)
                {
                    await Task.Delay(30);
                    SpawnSegment(item.prefab);
                }
            }

            OnLevelGenerationCompleted?.Invoke();
        }
    }

    private void SpawnSegment(GameObject segmentPrefab)
    {
        Vector3 spawnPosition = CalculateSpawnPosition();

        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, segmentParent.transform.rotation, segmentParent.transform);

        //newSegment.transform.SetParent(segmentParent.transform);

        spawnedSegments.Add(newSegment);

        totalLevelHeight += GetSegmentHeight(newSegment);

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

        float totalSegmentHeight = 0f;

        Renderer[] renderers = segment.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            totalSegmentHeight += renderer.bounds.size.y;
        }

        return totalSegmentHeight;
    }
}
