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

public class Obstacles 
{
    private SegmentGrid grid;
    private List<ObstaclesInfo> allObstacles;

    public void Initialize(SegmentGrid grid, List<ObstaclesInfo> obstaclesList, int obstacleCount, int difficultyLevel)
    {
        this.grid = grid;
        this.allObstacles = obstaclesList;

        List<ObstaclesInfo> filtered = allObstacles.FindAll(o => o.difficulty == difficultyLevel);

        for (int i = 0; i < obstacleCount; i++)
        {
            ObstaclesInfo config = filtered[Random.Range(0, filtered.Count)];
            
            //if (TryPlaceObstacle(config, out Vector3 pos))
            //{
            //    GameObject container = new GameObject("ObstacleContainer_" + i);
            //    container.transform.SetParent(transform);
            //    container.transform.position = pos;

            //    GameObject instance = Instantiate(config.prefab, pos, Quaternion.identity, container.transform);
            //    instance.SetActive(true);
            //}
        }
    }

    //private bool TryPlaceObstacle(ObstaclesInfo config, out Vector3 position)
    //{
    //    // TODO test
    //    int width = 4; 
    //    int height = 2;
    //    int maxTries = 10;

    //    for (int attempt = 0; attempt < maxTries; attempt++)
    //    {
    //        int x = Random.Range(0, grid.height - height + 1);
    //        int y = Random.Range(0, grid.width - width + 1);

    //        if (!grid.CanPlaceObstacle(x, y, width, height))
    //            continue;

    //        grid.PlaceObstacle(x, y, width, height);

    //        position = grid.GridToWorldPosition(x, y);
    //        return true;
    //    }

    //    position = Vector3.zero;
    //    return false;
    //}
}
