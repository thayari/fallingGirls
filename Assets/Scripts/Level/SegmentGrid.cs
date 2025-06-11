using UnityEngine;

public class SegmentGrid
{
    private int height, width;
    public Cell[,] cells;

    public int subdiv = 2;

    public SegmentGrid(float width, float height)
    {
        this.height = Mathf.CeilToInt(height * subdiv);
        this.width = Mathf.CeilToInt(width * subdiv);
        Debug.Log(this.width + ", " + this.height);

        InitCells();
    }

    private void InitCells()
    {
        cells = new Cell[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[i, j] = new Cell(i, j);
            }
        }
    }

    private void PlaceObstacle() { }

}

public class Cell
{
    public enum CellState
    {
        Empty,
        Obstacle
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