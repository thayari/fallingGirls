using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SegmentInfo
{
    public GameObject prefab;
    public int count;
}

[System.Serializable]
public class ObstaclesInfo
{
    public GameObject prefab;
    public int difficulty;
}

[System.Serializable]
public class DifficultyZonesInfo
{
    public int difficultyLevel;
    public int zoneHeight;
    public int obstaclesCount; 
}


[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private List<SegmentInfo> sequence;
    [SerializeField] private List<ObstaclesInfo> obstacles;
    [SerializeField] private List<DifficultyZonesInfo> difficultyZones;

    public List<SegmentInfo> GetSequence()
    {
        return sequence;
    }

    public List<ObstaclesInfo> GetObstaclesList()
    {
        return obstacles;
    }

    public List<DifficultyZonesInfo> GetDifficultyZonesList()
    {
        return difficultyZones;
    }
}
