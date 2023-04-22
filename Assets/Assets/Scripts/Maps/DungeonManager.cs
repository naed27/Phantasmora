using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    readonly private int cellSize = 2;
    readonly private int walkerCount = 2;

    public Cell[,] grid;

    public int gridSize;
    private int rowCount;
    private int colCount;

    private Cell endPoint;
    private Walker[] walkers;

    public GameObject Player;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;

    void Start()
    {
        this.SetupMazeCells();
        this.NetworkNeighbors();
        this.DetermineEndPoint();
        this.SpawnWalkers();
        this.ActivateWalkers();
        this.ExpandEndPoint();
        this.DrawMaze();
    }

    private void SetupMazeCells()
    {
        // Setup values for properties
        if (this.gridSize < 20) this.gridSize = 20;
        if (this.gridSize > 50) this.gridSize = 50;
        this.rowCount = this.gridSize;
        this.colCount = this.gridSize;
        

        this.grid = new Cell[this.rowCount, this.colCount];

        for (int x = 0; x < this.rowCount; x++)
            for (int y = 0; y < this.colCount; y++)
            {
                Cell cell = new(x,y,this);
                if(Helper.IsEdgeOfGrid(x,y, this.rowCount, this.colCount))
                    cell.tile = "edge";
                else
                    cell.tile = "null";
                this.grid[x, y] = cell;
            }

    }
    private void NetworkNeighbors()
    {

        for (int x = 0; x < this.rowCount; x++)
            for (int y = 0; y < this.colCount; y++)
                this.grid[x, y].DetermineNeighbors();

    }

    private void DrawMaze()
    {

        Gizmos.color = Color.yellow;
        float xBase = -(this.rowCount) + (cellSize / 2);
        float yBase = (this.colCount) - (cellSize / 2);

        GameObject prefab = null;

        for (int y = 0; y < this.colCount; y++)
        {
            for (int x = 0; x < this.rowCount; x++)
            {
                Cell cell = grid[x, y];
                if (cell.IsWall() || cell.IsEdge())
                    prefab = this.wallPrefab;
                else if (cell.IsFloor() || cell.IsSpawnPoint())
                    prefab = this.floorPrefab;
                else if (cell.IsGoalPoint())
                    prefab = this.doorPrefab;

                Vector3 spawnPosition = new(xBase + (x*cellSize), yBase - (y*cellSize), 0);
                if (prefab)
                {
                    GameObject gameObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                    gameObject.name = "Cell("+x.ToString()+", "+y.ToString()+")";
                    gameObject.transform.SetParent(transform);
                }
                if (cell.IsSpawnPoint())
                {
                    this.Player.transform.position = spawnPosition;
                }
            }
        }

    }

    private void SpawnWalkers()
    {
        Walker[] walkers = new Walker[this.walkerCount];

        int[] x = Helper.GenerateRandomNumbersRange(1, this.rowCount - 2, this.walkerCount);
        int[] y = Helper.GenerateRandomNumbersRange(1, this.colCount - 2, this.walkerCount);

        for (int i = 0; i < this.walkerCount; i++)
            walkers[i] = new Walker(this.grid[x[i], y[i]], this);

        this.walkers = walkers;
    }

    private void DetermineEndPoint()
    {

        int n1 = Helper.GenerateRandomNumberEither(0, this.rowCount - 1);
        int n2 = Helper.GenerateRandomNumber(1, this.rowCount - 2);

        if (Helper.GenerateRandomBool())
        {
            int temp = n1;
            n1 = n2;
            n2 = temp;
        }

        this.endPoint = this.grid[n1, n2];
        this.endPoint.tile = "goal";
    }

    private void ActivateWalkers()
    {
        for (int i = 0; i < this.walkers.Length; i++)
            this.walkers[i].Walk();
    }


    public bool IsWithinGrid(int x, int y)
    {
        if(x > 0 && x < this.rowCount-1 && y > 0 && y < this.colCount-1)
            return true;
        return false;
    }

    private void ForceFloor(int x, int y, bool isPlayerSpawnPoint = false)
    {
        if(this.IsWithinGrid(x, y))
        {
            Cell cell = this.grid[x, y];

            if (isPlayerSpawnPoint)
                cell.tile = "spawn";
            else
                cell.tile = "floor";
        }
    }

    private void ExpandEndPoint()
    {
        (int x, int y) = this.endPoint.coordinates;

        if (y == 0 || y == this.colCount - 1) this.ForceFloor(x + 0, y - 1, true);
        if (y == 0 || y == this.colCount - 1) this.ForceFloor(x + 0, y + 1, true);
        if (x == 0 || x == this.rowCount - 1) this.ForceFloor(x + 1, y + 0, true);
        if (x == 0 || x == this.rowCount - 1) this.ForceFloor(x - 1, y + 0, true);
        this.ForceFloor(x - 1, y + 1);
        this.ForceFloor(x + 1, y + 1);
        this.ForceFloor(x + 1, y - 1);
        this.ForceFloor(x - 1, y - 1);
    }

}