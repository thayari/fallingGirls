using UnityEngine;
using System.Collections.Generic;

public class Segment : MonoBehaviour
{
    public float Height { get; private set; }
    public SegmentGrid Grid { get; private set; }
    private Transform parentTransform;
    private LevelConfig levelConfig;

    public void Initialize(float segmentHeight, float levelWidth, LevelConfig config, Transform segmentTransform, int difficulty, int obstacleCount)
    {
        Height = segmentHeight;
        levelConfig = config;
        parentTransform = segmentTransform;

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
        obstacleGenerator.Generate(Grid, obstaclesContainer, levelConfig, difficulty, obstacleCount);

        PlaceCoins(coinsContainer);
    }

    private void PlaceCoins(Transform coinsContainer)
    {
        GameObject coinPrefab = levelConfig.GetCoinPrefab();
        if (coinPrefab == null) return;

        int currentY = 1;
        int currentX = Mathf.RoundToInt(Grid.Width / 2f) - 1;
        int[] adjacentOffsets = { 0, -1, 1, -2, 2 };

        while (currentY < Grid.Height - 1)
        {
            bool placeFound = false;
            foreach (int offset in adjacentOffsets)
            {
                int x = currentX + offset;
                if (Grid.IsSafeForCoin(x, currentY))
                {
                    Grid.SetCellState(x, currentY, Cell.CellState.Coin);

                    Vector2 coinSize = coinPrefab.GetComponent<Collider2D>()?.bounds.size ?? Vector2.one / Grid.subdiv;
                    Vector3 position = Grid.GetWorldPositionFromGrid(new Vector2Int(x, currentY), coinSize);

                    Instantiate(coinPrefab, position, Quaternion.identity, coinsContainer);

                    currentX = x;
                    placeFound = true;
                    break;
                }
            }
            currentY++;
        }
    }
}