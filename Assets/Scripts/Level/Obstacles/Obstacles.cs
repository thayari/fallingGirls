using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacles
{
    private const int MIN_VERTICAL_GAP = 10;
    private const int MAX_VERTICAL_GAP = 15;

    public void Generate(SegmentGrid grid, Transform parentTransform, LevelConfig levelConfig, int difficulty, int count, float initialContentOffset = 0f)
    {
        if (count == 0) return;

        List<GameObject> validPrefabs = levelConfig.GetObstaclesList()
                                                     .Where(obs => obs.difficulty == difficulty)
                                                     .Select(obs => obs.prefab)
                                                     .ToList();
        if (validPrefabs.Count == 0) return;

        int gridOffsetY = Mathf.CeilToInt(initialContentOffset * grid.subdiv);

        int currentY = gridOffsetY + Random.Range(MIN_VERTICAL_GAP, MAX_VERTICAL_GAP + 1);
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

            Vector3 worldPos = grid.GetObstaclePosition(gridPos, colliderSize);

            Quaternion spawnRotation = Quaternion.identity;

            Vector3 spawnScale = placeOnLeft ? Vector3.one : new Vector3(-1f, 1f, 1f);

            GameObject spawnedObject = Object.Instantiate(prefabToSpawn, worldPos, spawnRotation, parentTransform);
            spawnedObject.transform.localScale = spawnScale;

            grid.MarkArea(xPosInGrid, currentY, gridWidth, gridHeight, Cell.CellState.Obstacle);

            placedCount++;
            currentY += gridHeight + Random.Range(MIN_VERTICAL_GAP, MAX_VERTICAL_GAP + 1);
        }
    }
}