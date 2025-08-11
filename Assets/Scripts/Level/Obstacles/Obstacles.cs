using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle
{
    public ObstaclesInfo config;
    public GameObject instance;
}

public class Obstacles
{
    private const int MIN_VERTICAL_GAP = 3;

    public void Generate(SegmentGrid grid, Transform parentTransform, LevelConfig levelConfig, int difficulty, int count)
    {
        if (count == 0) return;

        List<GameObject> validPrefabs = levelConfig.GetObstaclesList()
                                                     .Where(obs => obs.difficulty == difficulty)
                                                     .Select(obs => obs.prefab)
                                                     .ToList();

        if (validPrefabs.Count == 0) return;

        int currentY = MIN_VERTICAL_GAP;
        int placedCount = 0;

        while (placedCount < count)
        {
            GameObject prefabToSpawn = validPrefabs[Random.Range(0, validPrefabs.Count)];
            Collider2D collider = prefabToSpawn.GetComponent<Collider2D>();

            if (collider == null)
            {
                Debug.LogError($"Obstacle prefab '{prefabToSpawn.name}' must have a Collider2D component!");
                continue;
            }

            Vector2 colliderSize = collider.bounds.size;
            int gridWidth = Mathf.CeilToInt(colliderSize.x * grid.subdiv);
            int gridHeight = Mathf.CeilToInt(colliderSize.y * grid.subdiv);

            if (currentY + gridHeight >= grid.Height)
            {
                Debug.Log("Not enough space in segment to place all requested obstacles.");
                break;
            }

            bool placeOnLeft = Random.value > 0.5f;
            int xPos = placeOnLeft ? 0 : grid.Width - gridWidth;

            if (grid.IsAreaEmpty(xPos, currentY, gridWidth, gridHeight))
            {
                Vector3 worldPos = grid.GetWorldPositionFromGrid(new Vector2Int(xPos, currentY), parentTransform, colliderSize);
                Object.Instantiate(prefabToSpawn, worldPos, Quaternion.identity, parentTransform);

                grid.MarkArea(xPos, currentY, gridWidth, gridHeight, Cell.CellState.Obstacle);

                placedCount++;

                currentY += gridHeight + MIN_VERTICAL_GAP;
            }
            else
            {
                currentY++;
            }
        }
    }
}