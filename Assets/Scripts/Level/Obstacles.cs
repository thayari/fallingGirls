﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ObstacleInfo
{
    public GameObject prefab;
}


public class Obstacles : MonoBehaviour
{
    [SerializeField] private int obstaclesCount = 100;
    [SerializeField] private float generateDistance = 5f;
    [SerializeField] private float removeDistance = 2f;
    [SerializeField] private float offset = 100f; // offset from the bottom of a level, where obstacles start to appear
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
        Debug.Log("Init obstacles generation");
    }

    private void OnDestroy()
    {
        level.OnLevelGenerationCompleted -= HandleLevelGenerationCompleted;
    }

    private void Init()
    {
        LevelConfig levelConfig = level.GetLevelConfig();
        List<ObstaclesInfo> obstaclesList = levelConfig.GetObstaclesList();
        List<DifficultyZonesInfo> difficultyZonesList = levelConfig.GetDifficultyZonesList();

        if (obstaclesList.Count > 0 && difficultyZonesList.Count > 0)
        {
            for (int i = 0; i < difficultyZonesList.Count; i++)
            {
                DifficultyZonesInfo currentDifficultyZone = difficultyZonesList[i];

                List<ObstaclesInfo> currentObstacles = new List<ObstaclesInfo>();
                foreach (ObstaclesInfo obstacle in obstaclesList)
                {
                    if (obstacle.difficulty == currentDifficultyZone.difficultyLevel)
                    {
                        currentObstacles.Add(obstacle);
                    }
                }

                Debug.Log("Difficulty " + i);

            }
            //float intervalSize = (level.TotalLevelHeight - offset) / obstaclesCount;

            //float currentYPosition = offset;

            //    for (int i = 0; i < obstaclesCount; i++)
            //    {
            //        GameObject container = new GameObject("ObstacleContainer_" + i);

            //        container.transform.SetParent(transform);
            //        container.transform.position = new Vector3(0, currentYPosition, transform.position.z);


            //        GameObject item = obstaclesList[Random.Range(0, obstaclesList.Count)];

            //        obstacleContainers.Add(new Tuple<Transform, GameObject>(container.transform, item));

            //        currentYPosition += intervalSize;
            //    }
        }
    }

    void Update()
    {
        //GenerateObstacles();
        //RemoveObstacles();
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

                    obstacleInstance.transform.eulerAngles = new Vector3(0, Random.Range(0, 2) == 0 ? 180 : 0, 0); // Mirror to left
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
