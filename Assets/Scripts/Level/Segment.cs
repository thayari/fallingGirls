using UnityEngine;
using System.Collections.Generic;

public class Segment : MonoBehaviour
{
    public float Height { get; private set; }
    public SegmentGrid Grid { get; private set; }

    private Transform parentTransform;
    private LevelConfig levelConfig;
    private float initialContentOffset = 0f;

    public void Initialize(float segmentHeight, float levelWidth, LevelConfig config, Transform segmentTransform, int difficulty = 0, int obstacleCount = 0, float startOffset = 0f)
    {
        Height = segmentHeight;
        levelConfig = config;
        parentTransform = segmentTransform;
        initialContentOffset = startOffset;

        Grid = new SegmentGrid(levelWidth, segmentHeight, segmentTransform);

        GenerateContent(difficulty, obstacleCount);
    }

    private void GenerateContent(int difficulty, int obstacleCount)
    {
        Transform obstaclesContainer = new GameObject("[Obstacles]").transform;
        obstaclesContainer.SetParent(parentTransform, false);

        Transform coinsContainer = new GameObject("[Coins]").transform;
        coinsContainer.SetParent(parentTransform, false);

        Obstacles obstacleGenerator = new Obstacles();
        obstacleGenerator.Generate(Grid, obstaclesContainer, levelConfig, difficulty, obstacleCount, initialContentOffset);

        PlaceCoins(coinsContainer, initialContentOffset);
    }

    private void PlaceCoins(Transform coinsContainer, float initialContentOffset)
    {
        const int COIN_DISTANCE = 3;

        GameObject coinPrefab = levelConfig.GetCoinPrefab();
        if (coinPrefab == null) return;

        Vector2 coinWorldSize;
        Collider2D coinCollider = coinPrefab.GetComponent<Collider2D>();
        if (coinCollider != null)
        {
            coinWorldSize = coinCollider.bounds.size;
        }
        else
        {
            Debug.LogWarning("Coin prefab is missing a Collider2D. Using default size.");
            coinWorldSize = Vector2.one * (1f / Grid.subdiv);
        }

        int coinGridHeight = Mathf.Max(1, Mathf.CeilToInt(coinWorldSize.y * Grid.subdiv));

        int gridOffsetY = Mathf.CeilToInt(initialContentOffset * Grid.subdiv);
        int currentY = gridOffsetY + 1;

        int targetX = Mathf.RoundToInt(Grid.Width / 2f);

        int verticalStep = coinGridHeight + (COIN_DISTANCE * coinGridHeight);
        if (verticalStep == 0) verticalStep = 1; 

        while (currentY < Grid.Height - 1)
        {
            bool placeFound = false;
            int finalX = -1; 

            if (Grid.IsSafeForCoin(targetX, currentY))
            {
                finalX = targetX;
                placeFound = true;
            }
            else
            {
                for (int offset = 1; offset < Grid.Width / 2; offset++)
                {
                    int rightX = targetX + offset;
                    if (Grid.IsSafeForCoin(rightX, currentY))
                    {
                        finalX = rightX;
                        placeFound = true;
                        break; 
                    }

                    int leftX = targetX - offset;
                    if (Grid.IsSafeForCoin(leftX, currentY))
                    {
                        finalX = leftX;
                        placeFound = true;
                        break; 
                    }
                }
            }

            if (placeFound)
            {
                Grid.SetCellState(finalX, currentY, Cell.CellState.Coin);

                Vector3 position = Grid.GetWorldPositionFromGrid(new Vector2Int(finalX, currentY), coinWorldSize);
                Instantiate(coinPrefab, position, Quaternion.identity, coinsContainer);

                targetX = finalX;
            }

            currentY += verticalStep;
        }
    }

    private void OnDrawGizmos()
    {
        Grid.DrawGridGizmos();
    }
}