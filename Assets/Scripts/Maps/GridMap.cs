using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    Cell[,] grid;

    public int cellSize;
    public int rowCount;
    public int colCount;

    public GameObject wallPrefab;
    public GameObject floorPrefab;

    public void Initialize(int rowCount, int colCount)
    {
        grid = new Cell[rowCount, colCount];
        this.rowCount = rowCount;
        this.colCount = colCount;

        for (int x = 0; x < rowCount; x++)
        {
            for (int y = 0; y < colCount; y++)
            {
                Cell cell = new();
                if(this.IsEdgeOfGrid(x,y,rowCount,colCount))
                    cell.isWall = true;
                else
                    cell.isWall = false;
                grid[x, y] = cell;
            }
        }

    }

    public void DrawMaze()
    {

        Gizmos.color = Color.yellow;
        float xBase = -(rowCount) + (cellSize / 2);
        float yBase = (colCount) - (cellSize / 2);

        GameObject prefab;

        for (int y = 0; y < colCount; y++)
        {
            for (int x = 0; x < rowCount; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWall)
                    prefab = this.wallPrefab;
                else
                    prefab = this.floorPrefab;

                Vector3 spawnPosition = new(xBase + (x*cellSize), yBase - (y*cellSize), 0);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }

    }

    bool IsEdgeOfGrid(int x, int y, int gridWidth, int gridHeight)
    {
        bool isOnEdge = false;

        if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1)
        {
            isOnEdge = true;
        }

        return isOnEdge;
    }

}