using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInfo
{
    public GameObject prefab;
    public int count;
}


public class Obstacles : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float generateDistance = 50f;
    [SerializeField] private float removeDistance = 10f;
    [SerializeField] private Level level;
    [SerializeField] private List<ObstacleInfo> sequence;
    
    void Update()
    {
        GenerateObstacles();
        RemoveObstacles();
    }

    void GenerateObstacles()
    {
        
    }

    void RemoveObstacles()
    {
        
    }
}
