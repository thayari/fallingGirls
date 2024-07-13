using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] segmentPrefabs;
    public int numberOfSegments = 10;
    public Transform segmentParent;
    private List<GameObject> spawnedSegments = new List<GameObject>();

    private void Awake()
    {
        Init();
    }

    private async void Init()
    {
        for (int i = 0; i < numberOfSegments; i++)
        {
            await Task.Delay(30);
            SpawnSegment();
        }
    }

    private void SpawnSegment()
    {
        GameObject segmentPrefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Length)];

        Vector3 spawnPosition = CalculateSpawnPosition();

        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);

        newSegment.transform.SetParent(segmentParent.transform);

        spawnedSegments.Add(newSegment);
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

        float totalHeight = 0f;

        MeshRenderer[] meshRenderers = segment.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            totalHeight += meshRenderer.bounds.size.y;
        }

        return totalHeight;
    }
}
