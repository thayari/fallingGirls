using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Obstacle
{
    public ObstaclesInfo config;
    public float positionY;
    public GameObject container;
    public GameObject item;
}

public class Obstacles : MonoBehaviour
{
    [SerializeField] private int obstaclesCount = 100;
    [SerializeField] private float generateDistance = 5f;
    [SerializeField] private float removeDistance = 2f;
    [SerializeField] private float offset = 5f; // offset from the bottom of a level, where obstacles start to appear
    [SerializeField] private Level level;

    private List<Tuple<Transform, GameObject>> obstacleContainers;
    private Transform playerTransform;
    private List<Obstacle> obstacles;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        obstacleContainers = new List<Tuple<Transform, GameObject>>();
        obstacles = new List<Obstacle>();


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
        LevelConfig levelConfig = level.GetLevelConfig();
        List<ObstaclesInfo> obstaclesList = levelConfig.GetObstaclesList();
        List<DifficultyZonesInfo> difficultyZonesList = levelConfig.GetDifficultyZonesList();

        if (obstaclesList.Count > 0 && difficultyZonesList.Count > 0)
        {
            float positionY = offset;
            for (int i = 0; i < difficultyZonesList.Count; i++)
            {
                DifficultyZonesInfo currentDifficultyZone = difficultyZonesList[i];
                float distance = currentDifficultyZone.zoneHeight / currentDifficultyZone.obstaclesCount;

                List<ObstaclesInfo> currentObstacles = new List<ObstaclesInfo>();

                foreach (ObstaclesInfo obstacle in obstaclesList)
                {
                    if (obstacle.difficulty == currentDifficultyZone.difficultyLevel)
                    {
                        currentObstacles.Add(obstacle);
                    }
                }

                for (int j = 0; j < currentDifficultyZone.obstaclesCount; j++)
                {
                    int randomIndex = Random.Range(0, currentObstacles.Count);
                    obstacles.Add(new Obstacle
                    {
                        config = currentObstacles[randomIndex],
                        positionY = positionY,
                    });
                    positionY += distance;
                }


            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                GameObject container = new GameObject("ObstacleContainer_" + i);
                Transform containerTransform = container.transform;

                containerTransform.SetParent(transform);
                containerTransform.position = new Vector3(0, obstacles[i].positionY, transform.position.z);

                obstacles[i].container = container;

                GameObject obstaclePrefab = obstacles[i].config.prefab;
                GameObject obstacleInstance = Instantiate(obstaclePrefab, containerTransform.position, containerTransform.rotation, containerTransform);

                obstacles[i].item = obstacleInstance;

                obstacleInstance.transform.eulerAngles = new Vector3(0, Random.Range(0, 2) == 0 ? 180 : 0, 0); // Mirror to left

                obstacleInstance.SetActive(false);

                GetColliderSize(obstacleInstance);
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
        foreach (var obstacle in obstacles)
        {
            if (Vector3.Distance(playerTransform.position, obstacle.container.transform.position) <= generateDistance)
            {
                obstacle.item.SetActive(true);
            }
        }
    }

    void RemoveObstacles()
    {
        foreach (var obstacle in obstacles)
        {
            if (Vector3.Distance(playerTransform.position, obstacle.container.transform.position) >= generateDistance + removeDistance)
            {
                obstacle.item.SetActive(false);
            }
        }
    }

    void GetColliderSize(GameObject obstacleInstance)
    {
        CapsuleCollider2D capsuleCollider2D = obstacleInstance.GetComponentInChildren<CapsuleCollider2D>();
        if (capsuleCollider2D != null)
        {
            float width = capsuleCollider2D.size.x; // Width of the collider
            float height = capsuleCollider2D.size.y; // Height of the collider

            // Log the values
            Debug.Log("Width: " + width + ", Height: " + height);
        }
        else
        {
            Debug.LogError("CapsuleCollider2D not found on the child object.");
        }
    }
}
