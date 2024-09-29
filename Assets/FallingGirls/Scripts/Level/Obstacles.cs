using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ObstacleInfo
{
    public GameObject prefab;
}


public class Obstacles : MonoBehaviour
{
    [SerializeField] private List<ObstacleInfo> obstaclesList;
    [SerializeField] private int obstaclesCount = 100;
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
        if (obstaclesList.Count > 0)
        {
            float intervalSize = (level.TotalLevelHeight - offset) / obstaclesCount;

            float currentYPosition = offset;

                for (int i = 0; i < obstaclesCount; i++)
                {
                    GameObject container = new GameObject("ObstacleContainer_" + i);

                    container.transform.SetParent(this.transform);
                    container.transform.position = new Vector3(0, currentYPosition, this.transform.position.z);


                    ObstacleInfo item = obstaclesList[UnityEngine.Random.Range(0, obstaclesList.Count)];

                    obstacleContainers.Add(new Tuple<Transform, GameObject>(container.transform, item.prefab));

                    currentYPosition += intervalSize;
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

                    obstacleInstance.transform.localScale = new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1, 1, 1); // Mirror to left
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
