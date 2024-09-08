using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ObstacleInfo
{
    public GameObject prefab;
    public int count;
}


public class Obstacles : MonoBehaviour
{
    [SerializeField] private List<ObstacleInfo> sequence;
    [SerializeField] private float generateDistance = 5f;
    [SerializeField] private float removeDistance = 2f;
    [SerializeField] private float offset = 100f;
    [SerializeField] private Level level;

    private List<Tuple<Transform, GameObject>> obstacleContainers;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        obstacleContainers = new List<Tuple<Transform, GameObject>>();

        level.OnLevelGenerationCompleted += HandleLevelGenerationCompleted;
    }

    private void HandleLevelGenerationCompleted()
    {
        Init();
    }

    private void OnDestroy()
    {
        level.OnLevelGenerationCompleted -= HandleLevelGenerationCompleted;
    }

    private void Init()
    {
        if (sequence.Count > 0)
        {
            int obstaclesCount = sequence.Sum(s => s.count);

            float intervalSize = (level.TotalLevelHeight - offset) / obstaclesCount;

            float currentYPosition = offset;

            foreach (var item in sequence)
            {
                for (int i = 0; i < item.count; i++)
                {
                    GameObject container = new GameObject("ObstacleContainer_" + i);

                    container.transform.position = new Vector3(0, currentYPosition, 0);

                    container.transform.SetParent(this.transform);

                    obstacleContainers.Add(new Tuple<Transform, GameObject>(container.transform, item.prefab));

                    currentYPosition += intervalSize;
                }
            }
        }
    }

    void Update()
    {
        GenerateObstacles();
        RemoveObstacles();
    }

    void GenerateObstacles()
    {
        foreach (var obstacleData in obstacleContainers)
        {
            Transform containerTransform = obstacleData.Item1;
            GameObject obstaclePrefab = obstacleData.Item2;

            if (Vector3.Distance(playerTransform.position, containerTransform.position) <= generateDistance)
            {
                if (containerTransform.childCount == 0)
                {
                    GameObject obstacleInstance = Instantiate(obstaclePrefab, containerTransform.position, containerTransform.rotation, containerTransform);
                }
            }
        }
    }

    void RemoveObstacles()
    {
        foreach (var obstacleData in obstacleContainers)
        {
            Transform containerTransform = obstacleData.Item1;

            if (Vector3.Distance(playerTransform.position, containerTransform.position) >= generateDistance + removeDistance)
            {
                if (containerTransform.childCount > 0)
                {
                    Destroy(containerTransform.GetChild(0).gameObject);
                }
            }
        }
    }
}
