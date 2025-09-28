using UnityEngine;

public class SegmentGrid
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Cell[,] cells;

    public readonly int subdiv = 2;
    private readonly Vector3 segmentWorldOrigin;
    private readonly float cellWorldSize;
    private readonly float segmentWorldWidth;
    private readonly float rawWidth;
    private readonly float rawHeight;

    public SegmentGrid(float width, float height, Transform segmentTransform)
    {
        this.rawWidth = width;
        this.rawHeight = height;
        this.Width = Mathf.CeilToInt(width * subdiv);
        this.Height = Mathf.CeilToInt(height * subdiv);
        this.cellWorldSize = 1.0f / subdiv;

        this.segmentWorldWidth = this.Width * this.cellWorldSize;
        float segmentWorldHeight = this.Height * this.cellWorldSize;

        Vector3 segmentCenter = segmentTransform.position;
        this.segmentWorldOrigin = segmentCenter - new Vector3(segmentWorldWidth / 2.0f, 0, 0);

        InitCells();
    }

    private void InitCells()
    {
        cells = new Cell[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                cells[i, j] = new Cell(j, i);
            }
        }
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int gridPos, Vector2 objectWorldSize)
    {
        // 1. Вычисляется левый нижний угол ячейки gridPos
        float localX = gridPos.x * cellWorldSize;
        float localY = gridPos.y * cellWorldSize;

        // 2. К этому углу прибавляется половина размера объекта
        Vector3 objectCenterOffset = new Vector3(localX + objectWorldSize.x / 2.0f, localY + objectWorldSize.y / 2.0f, 0);

        // 3. Результат смещается на позицию самого грида
        return segmentWorldOrigin + objectCenterOffset;
    }

    public Vector3 GetObstaclePosition(Vector2Int gridPos, Vector2 objectWorldSize)
    {
        float localX = gridPos.x * cellWorldSize;
        float localY = gridPos.y * cellWorldSize;

        float targetX;
        float targetY = localY + objectWorldSize.y / 2.0f;

        if (gridPos.x == 0)
        {
            // пивот на расстоянии половины ширины от левого края
            targetX = objectWorldSize.x / 2.0f;
        }
        else
        {
            targetX = rawWidth - (objectWorldSize.x / 2.0f);
        }

        Vector3 objectCenterOffset = new Vector3(targetX, targetY, 0);
        return segmentWorldOrigin + objectCenterOffset;
    }


    public void SetCellState(int x, int y, Cell.CellState state)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            cells[y, x].SetState(state);
        }
    }

    public void MarkArea(int startX, int startY, int width, int height, Cell.CellState state)
    {
        for (int y = startY; y < startY + height; y++)
        {
            for (int x = startX; x < startX + width; x++)
            {
                SetCellState(x, y, state);
            }
        }
    }

    public bool IsAreaEmpty(int startX, int startY, int width, int height)
    {
        if (startX < 0 || startY < 0 || startX + width > Width || startY + height > Height) return false;
        for (int y = startY; y < startY + height; y++)
        {
            for (int x = startX; x < startX + width; x++)
            {
                if (cells[y, x].state != Cell.CellState.Empty) return false;
            }
        }
        return true;
    }

    public bool IsSafeForCoin(int x, int y)
    {
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int checkX = x + dx;
                int checkY = y + dy;
                if (checkX < 0 || checkX >= Width || checkY < 0 || checkY >= Height) return false;
                if (cells[checkY, checkX].state != Cell.CellState.Empty) return false;
            }
        }
        return true;
    }

    public void DrawGridGizmos()
    {
        if (cells == null) return; // Проверка на случай, если сетка еще не создана

        Vector3 cellSize = new Vector3(cellWorldSize, cellWorldSize, 0.1f);
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Vector3 cellCenter = GetWorldPositionFromGrid(new Vector2Int(j, i), new Vector2(cellWorldSize, cellWorldSize));

                switch (cells[i, j].state)
                {
                    case Cell.CellState.Empty:
                        Gizmos.color = Color.white;
                        break;
                    case Cell.CellState.Obstacle:
                        Gizmos.color = Color.red;
                        break;
                    case Cell.CellState.Coin:
                        Gizmos.color = Color.yellow;
                        break;
                }

                // Рисуем полупрозрачный куб на месте ячейки
                Gizmos.DrawWireCube(cellCenter, cellSize);
            }
        }
    }
}

public class Cell
{
    public enum CellState
    {
        Empty,
        Obstacle,
        Coin
    }

    public CellState state;
    public Vector2Int gridPosition;

    public Cell(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
        state = CellState.Empty;
    }

    public void SetState(CellState newState)
    {
        state = newState;
    }
}