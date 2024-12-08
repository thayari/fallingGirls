using UnityEngine;

public class LevelGrid
{
    private int height, width;
    public Cell[,] cells;

    public LevelGrid(float width, float height)
    {
        this.height = Mathf.CeilToInt(height);
        this.width = Mathf.CeilToInt(width);

        InitCells();
    }

    private void InitCells()
    {
        cells = new Cell[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                cells[i, j] = new Cell();
            }
        }
    }

}

public class Cell
{
    public bool hasObstacle = false, isFree = true;


}