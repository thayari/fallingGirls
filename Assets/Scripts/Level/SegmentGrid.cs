using UnityEngine;
using System.Collections.Generic;
using System;


public class SegmentGrid 
{
    private int height, width;
    public Cell[,] cells;

    public int subdiv = 2;

    public const int MIN_GAP = 2;
    public const int MAX_GAP = 4;

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


    private void CreateObstacles()
    {

        int currentY = MIN_GAP;

        List<Obstacle> obstacles = new List<Obstacle> ();


        while (currentY < this.height)
        {
            Obstacle obstacle = new Obstacle();

            int position = Random.Range(0, 2);

            obstacle.left = position == 1;


            for (int y = 0; y < obstacle.height; y++)
            {
                for (int x = 0; x < obstacle.width; x++)
                {

                    if (obstacle.left)
                    {
                        if (x < this.width) 
                            cells[currentY, x].SetState(Cell.CellState.Obstacle);
                        
                    }
                    else
                    {
                        int rightPos = this.width - 1 - x;
                        if (rightPos >= 0)
                            cells[currentY, rightPos].SetState(Cell.CellState.Obstacle);

                    }
                }
                currentY++;

                if (currentY >= this.height) break;

            }

            currentY += Random.Range(MIN_GAP, MAX_GAP + 1); 

        }
    }

    private void CreateCoins()
    {
        int coinsGap = 1; // TODO 

        int currentY = 1;

        int currentX = Mathf.RoundToInt(this.width / 2f) - 1;


        while (currentY < this.height)
        {
            int[] adjacentPositions = { 0, -1, 1, -2, 2, -3, 3 };
            bool isPlaceFound = false;


            foreach (int num in adjacentPositions)
            {
                int x = currentX + num;


                bool cellFree = IsSafeForCoin(x, currentY);
                if (cellFree && !isPlaceFound)
                {
                    cells[currentY, x].SetState(Cell.CellState.Coin);
                    currentX = x;
                    isPlaceFound = true;
                }
            }
            currentY++;

        }
    }

    /// <summary>
    /// Проверяет, можно ли разместить монету в указанной ячейке.
    /// </summary>
    /// <param name="x">Координата X для проверки.</param>
    /// <param name="y">Координата Y для проверки.</param>
    /// <returns>True, если ячейка и все ее 8 соседей пусты.</returns>
    private bool IsSafeForCoin(int x, int y)
    {
        if (x < 0 || y < 0 || x >= this.width || y >= this.height)
        {
            return false;

        }


        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int checkX = x + dx;
                int checkY = y + dy;


                if ((checkX >= 0 && checkX < this.width) && (checkY >= 0 && checkY < this.height))
                {
                    if (cells[checkY, checkX].state != Cell.CellState.Empty)
                    {
                        return false;
                    }
                }
                else
                {
                    // Если соседняя ячейка выходит за пределы поля (например, для монет у края),
                    // считаем это место небезопасным, чтобы монеты не появлялись вплотную к границам.
                    return false;
                }
            }
        }

        return true;

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