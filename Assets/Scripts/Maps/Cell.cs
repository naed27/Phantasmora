using UnityEngine;
using System.Collections.Generic;

public class Cell
{
    public string id;
    public string tile;

    public Coordinate coordinates;
    public GridMap gridMap;
    public List<Cell> neighbors = new();
    public int wallInstance = 0;


    public Cell(int x, int y, GridMap gridMap)
    {
        this.id = "(" + x.ToString()+ ", " + y.ToString() +")";
        this.coordinates = new Coordinate(x, y);
        this.gridMap = gridMap;
    }

    public void DetermineNeighbors()
    {
        (int x, int y) = this.coordinates;
        if (this.gridMap.IsWithinGrid(x - 1, y)) neighbors.Add(this.gridMap.grid[x - 1, y]);
        if (this.gridMap.IsWithinGrid(x + 1, y)) neighbors.Add(this.gridMap.grid[x + 1, y]);
        if (this.gridMap.IsWithinGrid(x, y - 1)) neighbors.Add(this.gridMap.grid[x, y - 1]);
        if (this.gridMap.IsWithinGrid(x, y + 1)) neighbors.Add(this.gridMap.grid[x, y + 1]);
    }


    public bool IsEdge() { return this.tile == "edge"; }

    public bool IsWall() { return this.tile == "wall"; }

    public bool IsFloor() { return this.tile == "floor"; }

    public bool IsNull() { return this.tile == "null"; }

    public bool IsGoalPoint() { return this.tile == "goal"; }

    public bool IsSpawnPoint() { return this.tile == "spawn"; }

    public bool IsSameCell(Cell cell) { return cell != null && this.id == cell.id; }


    public List<Cell> GetNeighbors() { return this.neighbors; }

}
