using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            CapsuleCollider2D collider = prefabToSpawn.GetComponent<CapsuleCollider2D>();
            if (collider == null) continue;

            Vector2 localSize = collider.size;

            Vector2 worldSize = new Vector2(
                localSize.x * prefabToSpawn.transform.lossyScale.x,
                localSize.y * prefabToSpawn.transform.lossyScale.y
            );

            Vector2 colliderSize = worldSize;

            int gridWidth = Mathf.CeilToInt(colliderSize.x * grid.subdiv);
            int gridHeight = Mathf.CeilToInt(colliderSize.y * grid.subdiv);

            if (currentY + gridHeight >= grid.Height) break;

            bool placeOnLeft = Random.value > 0.5f;
            int xPosInGrid = placeOnLeft ? 0 : grid.Width - gridWidth;

            Vector2Int gridPos = new Vector2Int(xPosInGrid, currentY);

            Vector3 worldPos = grid.GetWorldPositionFromGrid(gridPos, colliderSize);

            Object.Instantiate(prefabToSpawn, worldPos, Quaternion.identity, parentTransform);

            grid.MarkArea(xPosInGrid, currentY, gridWidth, gridHeight, Cell.CellState.Obstacle);

            placedCount++;
            currentY += gridHeight + MIN_VERTICAL_GAP;
        }
    }
}