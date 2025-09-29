using UnityEngine;
using System.Collections.Generic;

public class Segment : MonoBehaviour
{
    public float Height { get; private set; }
    public SegmentGrid Grid { get; private set; }

    private Transform parentTransform;
    private LevelConfig levelConfig;
    private float initialContentOffset = 0f;

    [Header("Coin Placement Settings")]
    [Tooltip("Как часто изгибается волна монет. Рекомендуемые значения: 0.1 - 0.5")]
    public float coinWaveFrequency = 0.2f;

    [Tooltip("Насколько сильно волна отклоняется от центра (в ячейках сетки). Рекомендуемые значения: 2.0 - 5.0")]
    public float coinWaveAmplitude = 3.0f;

    private float wavePhaseShift;

    public void Initialize(float segmentHeight, float levelWidth, LevelConfig config, Transform segmentTransform, int difficulty = 0, int obstacleCount = 0, float obstaclesStartOffset = 0f)
    {
        Height = segmentHeight;
        levelConfig = config;
        parentTransform = segmentTransform;
        initialContentOffset = obstaclesStartOffset;

        Grid = new SegmentGrid(levelWidth, segmentHeight, segmentTransform);

        GenerateContent(difficulty, obstacleCount);
    }

    private void GenerateContent(int difficulty, int obstacleCount)
    {
        wavePhaseShift = Random.Range(0f, 2f * Mathf.PI);

        Transform obstaclesContainer = new GameObject("[Obstacles]").transform;
        obstaclesContainer.SetParent(parentTransform, false);

        Transform coinsContainer = new GameObject("[Coins]").transform;
        coinsContainer.SetParent(parentTransform, false);

        Obstacles obstacleGenerator = new Obstacles();
        obstacleGenerator.Generate(Grid, obstaclesContainer, levelConfig, difficulty, obstacleCount, initialContentOffset);

        PlaceCoins(coinsContainer);
    }

    /// <summary>
    /// Проверяет, свободна ли область вокруг указанной ячейки, включая вертикальные отступы.
    /// </summary>
    /// <param name="centerX">Координата X центральной ячейки.</param>
    /// <param name="centerY">Координата Y центральной ячейки.</param>
    /// <param name="verticalPadding">Количество ячеек для проверки сверху и снизу.</param>
    /// <returns>True, если вся область безопасна.</returns>
    private bool IsAreaSafe(int centerX, int centerY, int verticalPadding)
    {
        // Проверяем диапазон ячеек по вертикали
        for (int y = centerY - verticalPadding; y <= centerY + verticalPadding; y++)
        {
            // Убедимся, что не выходим за границы сетки
            if (y >= 0 && y < Grid.Height)
            {
                if (!Grid.IsSafeForCoin(centerX, y))
                {
                    return false; // Найдено препятствие в области, это место небезопасно
                }
            }
        }
        return true; // Вся область свободна
    }

    private void PlaceCoins(Transform coinsContainer)
    {
        const int COIN_DISTANCE = 4;
        const int VERTICAL_PADDING = 3;

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
        //int gridOffsetY = Mathf.CeilToInt(initialContentOffset * Grid.subdiv);
        //int currentY = gridOffsetY + 1;
        int currentY = 1;

        float gridCenter = Grid.Width / 2f;
        int verticalStep = coinGridHeight + (COIN_DISTANCE * coinGridHeight);
        if (verticalStep == 0) verticalStep = 1;

        while (currentY < Grid.Height - (1 + VERTICAL_PADDING)) // Учитываем отступ при проверке границ цикла
        {
            float waveValue = Mathf.Sin((currentY * coinWaveFrequency) + wavePhaseShift);
            float waveOffset = waveValue * coinWaveAmplitude;
            int targetX = Mathf.RoundToInt(gridCenter + waveOffset);

            bool placeFound = false;
            int finalX = -1;

            targetX = Mathf.Clamp(targetX, 0, Grid.Width - 1);

            if (IsAreaSafe(targetX, currentY, VERTICAL_PADDING))
            {
                finalX = targetX;
                placeFound = true;
            }
            else
            {
                for (int offset = 1; offset < Grid.Width / 2; offset++)
                {
                    int rightX = targetX + offset;
                    if (rightX < Grid.Width && IsAreaSafe(rightX, currentY, VERTICAL_PADDING))
                    {
                        finalX = rightX;
                        placeFound = true;
                        break;
                    }

                    int leftX = targetX - offset;
                    if (leftX >= 0 && IsAreaSafe(leftX, currentY, VERTICAL_PADDING))
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
            }

            currentY += verticalStep;
        }
    }

    private void OnDrawGizmos()
    {
        if (Grid != null)
        {
            Grid.DrawGridGizmos();
        }
    }
}