using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    readonly private int cellSize = 2;

    private Cell[,] grid;

    public int gridSize;
    private int rowCount;
    private int colCount;
    private int walkerCount;

    private Cell endPoint;
    private Cell[] walkers;

    public GameObject Player;
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    void Start()
    {
        this.PrepareMazeFoundation();
        this.SpawnWalkers();
        this.DetermineEndPoint();
        this.PrepareInnerWalls();
        this.ExpandEndPoint();
        this.DrawMaze();

    }

 

    private void PrepareMazeFoundation()
    {
        // Setup values for properties
        if (gridSize < 20) this.gridSize = 20;
        if (gridSize > 50) this.gridSize = 50;
        this.rowCount = this.gridSize;
        this.colCount = this.gridSize;

        float rawCalc = ((this.rowCount + this.colCount) / 2) / 3;
        this.walkerCount = (int)Math.Floor(rawCalc);
        this.grid = new Cell[this.rowCount, this.colCount];

        for (int x = 0; x < this.rowCount; x++)
        {
            for (int y = 0; y < this.colCount; y++)
            {
                Cell cell = new(x,y);
                if(Helper.IsEdgeOfGrid(x,y, this.rowCount, this.colCount))
                    cell.tile = "edge";
                else
                    cell.tile = "null";
                this.grid[x, y] = cell;
            }
        }

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
                if (cell.tile == "wall" || cell.tile == "edge")
                    prefab = this.wallPrefab;
                else if(cell.tile == "floor" || cell.tile == "end")
                    prefab = this.floorPrefab;

                Vector3 spawnPosition = new(xBase + (x*cellSize), yBase - (y*cellSize), 0);
                if (prefab)
                {
                    GameObject gameObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                    gameObject.name = "Cell("+x.ToString()+", "+y.ToString()+")";
                }
                if (cell.tile == "end")
                {
                    this.Player.transform.position = spawnPosition;
                }
            }
        }

    }

    private void SpawnWalkers()
    {
        Cell[] walkers = new Cell[this.walkerCount];

        int[] xIndexes = Helper.GenerateRandomNumbersRange(1, this.rowCount - 2, this.walkerCount);
        int[] yIndexes = Helper.GenerateRandomNumbersRange(1, this.colCount - 2, this.walkerCount);

        for (int i = 0; i < this.walkerCount; i++)
        {
            walkers[i] = this.grid[xIndexes[i], yIndexes[i]];
        }

        this.walkers = walkers;
    }

    private void DetermineEndPoint()
    {

        int n1 = Helper.GenerateRandomNumberEither(0, this.rowCount - 1);
        int n2 = Helper.GenerateRandomNumber(0, this.rowCount - 1);

        if (Helper.GenerateRandomBool())
        {
            int temp = n1;
            n1 = n2;
            n2 = temp;
        }

        this.endPoint = this.grid[n1, n2];
        this.endPoint.tile = "end";

    }

    private void PrepareInnerWalls()
    {
        for (int i = 0; i < this.walkers.Length; i++)
        {
            this.Walk(
                this.walkers[i],
                null,
                "walker_path_" + i.ToString(),
                true
            );
        }
    }

    private void Walk(Cell currentCell, Cell previousCell, string pathId, bool isAdvancing)
    {
        string consoleMessage = "";

        consoleMessage += "-----------------\n";
        consoleMessage += ((isAdvancing) ? "Advancing ":"Returning ") +"Cell #" + currentCell.id;

        if (currentCell.IsSameCell(this.endPoint)) {
            consoleMessage += " > This cell is the endpoint.";
            Debug.Log(consoleMessage);
            return;
        };

        currentCell.tile = "floor";

        Cell[] neighbors = Helper.FilterGrid(
            this.GetCellNeighbors(currentCell), 
            x => (isAdvancing) ?
               !x.IsEdge()
            && !x.IsDeadEnd()
            && !x.IsEndPoint()
            && !x.IsWithinPath(pathId)
            && !x.IsSameCell(previousCell)
            :
               !x.IsEdge()
            && !x.IsDeadEnd()
            && !x.IsEndPoint()
            && !x.IsWithinPath(pathId)
            && !x.IsSameCell(previousCell)
        );


        consoleMessage += "> Neighbors (" + neighbors.Length+") [";
        for (int n = 0; n < neighbors.Length; n++)
        {
            consoleMessage += neighbors[n].id;
            if (n != neighbors.Length - 1) consoleMessage += ", ";
        }
        consoleMessage += "]";

        if (neighbors.Length == 0)
        {

            consoleMessage += " > It's a dead end";
            currentCell.isDeadEnd = true;

            if (currentCell.previousCell == null) return;

            consoleMessage += " > Returning.";
            Walk(currentCell.previousCell, currentCell, pathId, false);
            Debug.Log(consoleMessage);
            return;
        }


        if(currentCell.pathId==null)
        {
            currentCell.pathId = pathId;
            currentCell.previousCell = previousCell;
        }

        int nextIndex = Helper.GenerateRandomNumber(0, neighbors.Length - 1);
        Cell nextCell = neighbors[nextIndex];
        Debug.Log(consoleMessage);

        Helper.RemoveAt(ref neighbors, nextIndex);
        this.PrepareWalls(neighbors, pathId);
        Walk(nextCell, currentCell, pathId, true);
        return;
 
    }

    private Cell[] GetCellNeighbors(Cell cell)
    {
        (int x, int y) = cell.coordinates;
        return new Cell[] {
           this.GetCell(x-1,y),
           this.GetCell(x+1,y),
           this.GetCell(x,y-1),
           this.GetCell(x,y+1)
        };
    }

    private bool IsValidCoordinates(int x, int y)
    {
        if(x < 0 || x >= this.rowCount || y < 0 || y >= colCount)
        {
            return false;
        }
        return true;
    }

    private Cell GetCell(int x, int y)
    {
        if(this.IsValidCoordinates(x, y))
        {
            return this.grid[x, y];
        }
        return null;
    }

    private void ForceFloor(int x, int y)
    {
        if (this.IsValidCoordinates(x, y))
        {
            this.grid[x, y].tile = "floor";
            return;
        }
        return;
    }

    private void ExpandEndPoint()
    {
        (int x, int y) = this.endPoint.coordinates;

        if (y == 0 || y == this.colCount - 1) this.ForceFloor(x + 0, y - 1);
        if (y == 0 || y == this.colCount - 1) this.ForceFloor(x + 0, y + 1);
        if (x == 0 || x == this.rowCount - 1) this.ForceFloor(x + 1, y + 0);
        if (x == 0 || x == this.rowCount - 1) this.ForceFloor(x - 1, y + 0);
        this.ForceFloor(x - 1, y + 1);
        this.ForceFloor(x + 1, y + 1);
        this.ForceFloor(x + 1, y - 1);
        this.ForceFloor(x - 1, y - 1);
    }

    private void PrepareWalls(Cell[] cells, string pathId)
    {
        for(int i = 0; i < cells.Length; i++)
        {
            Cell cell = cells[i];
            if (cell.IsNull()) {
                cell.tile = "wall";
            }
            else
            {
                if (Helper.GenerateRandomNumber(1, this.walkerCount-2) == 1)
                {
                    cell.tile = "wall";
                }
            }
            cell.pathId = pathId;
        }

    }
    


}