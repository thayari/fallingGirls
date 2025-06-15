using UnityEngine;

public class Segment : MonoBehaviour
{
    public float Height { get; private set; }
    public int Difficulty { get; private set; }
    public int ObstacleCount { get; private set; }
    public SegmentGrid Grid { get; private set; } 

    public Obstacles SegmentObstacles { get; private set; }

    public void Initialize(float segmentHeight, float levelWidth, LevelConfig levelConfig)
    {
        Height = segmentHeight;
        Grid = new SegmentGrid(levelWidth, segmentHeight);

        // test
        ObstacleCount = 4;
        Difficulty = 0;

        SegmentObstacles = new Obstacles();
        SegmentObstacles.Initialize(Grid, levelConfig.GetObstaclesList(), ObstacleCount, Difficulty);
    }
}