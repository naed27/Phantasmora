using UnityEngine;
using System.Collections.Generic;

public class Walker
{
    private string id;

    private GridMap grid;

    private Cell head;
    private Cell start;
    private Cell goalPoint;
    private bool isAdvancing;

    private int walkCount = 0;
    private int walkLimit = 50;
    private int retreatCount = 0;

    readonly private List<Cell> deadEnds = new();
    readonly private List<Cell> wallsCache = new();
    readonly private List<Cell> pathHistory = new();

    public Walker(Cell startPoint, GridMap grid)
    {
        this.id = "Walker_" + startPoint.id;
        this.start = startPoint;
        this.head = start;
        this.grid = grid;
        this.pathHistory.Add(start);
        this.isAdvancing = true;
    }


    // ------------------------- Main Function

    public void Walk()
    {
        head.tile = "floor";

        if (head.IsSameCell(goalPoint)){ return; }


        List<Cell> neighbors = Helper.FilterList(
            head.GetNeighbors(),
            cell => (isAdvancing) ?
                   !cell.IsEdge()
                && (deadEnds.FindIndex(item => item.id == cell.id) == -1)       // if cell is not listed in deadEnds list
                && (wallsCache.FindIndex(item => item.id == cell.id) == -1)     // if cell is not listed in wallsCache list
                && (pathHistory.FindIndex(item => item.id == cell.id) == -1)    // if cell is not listed in pathHistory list
                :
                   !cell.IsEdge()
                && cell.wallInstance == 1
                && (deadEnds.FindIndex(item => item.id == cell.id) == -1)       // if cell is not listed in deadEnds list
                && (pathHistory.FindIndex(item => item.id == cell.id) == -1)    // if cell is not listed in pathHistory list
        );

        

        if (IsDeadEnd(neighbors)) { Retreat(); return; }

        retreatCount = 0;
        isAdvancing = true;
        SurroundByWalls(head);
        DetermineNextHead(neighbors);

        if (!pathHistory.Contains(head)) { pathHistory.Add(head); }
        Walk();
        return;

    }

    // ------------------------- Simple Action Functions

    private void Retreat()
    {
        retreatCount++;
        deadEnds.Add(head);
        SurroundByWalls(head);
        // fall back (at first detection offset)
        if (retreatCount==1 && pathHistory.Count>0)
            pathHistory.RemoveAt(pathHistory.Count - 1);

        // fall back (consecutive detection)
        head = GetPreviousCell();
        if (head == null) return;
        isAdvancing = false;
        pathHistory.RemoveAt(pathHistory.Count - 1);
        Walk();
    }


    private void SurroundByWalls(Cell cell)
    {
        List<Cell> neighbors = cell.GetNeighbors();

        for (int i = 0; i < neighbors.Count; i++)
        {
            Cell neighbor = neighbors[i];

            if (!IsHead(neighbor) && (pathHistory.FindIndex(item => item.id == neighbor.id) == -1) && !neighbor.IsFloor())
            {
                neighbor.tile = "wall";
                neighbor.wallInstance++;
                wallsCache.Add(neighbor);
            }
        }

    }

    // ------------------------- Simple Functions

    private Cell GetPreviousCell() {
        if (pathHistory.Count == 0) return null;
        return pathHistory[^1];
    }

    private bool IsDeadEnd(List<Cell> neighbors) { return neighbors.Count == 0; }

    private bool IsHead(Cell cell) { return cell.id == head.id; }

    private void DetermineNextHead(List<Cell> n) { head = n[Helper.GenerateRandomNumber(0, n.Count - 1)]; }


}
