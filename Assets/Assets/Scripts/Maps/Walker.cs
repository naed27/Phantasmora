using UnityEngine;
using System.Collections.Generic;

public class Walker
{
    private string _id;

    private DungeonManager _grid;

    private Tile _head;
    readonly private Tile _start;
    readonly private Tile _goalPoint;

    private bool isAdvancing;
    private int retreatCount = 0;

    readonly private List<Tile> _deadEnds = new();
    readonly private List<Tile> _wallsCache = new();
    readonly private List<Tile> _pathHistory = new();

    public Walker(Tile startPoint, DungeonManager grid)
    {
        this._id = "Walker_" + startPoint.Id;
        this._start = startPoint;
        this._head = _start;
        this._grid = grid;
        this._pathHistory.Add(_start);
        this.isAdvancing = true;
    }


    // ------------------------- Main Function

    public void Walk()
    {
        _head.Type = "floor";

        if (_head.IsSameCell(_goalPoint)){ return; }


        List<Tile> neighbors = Helper.FilterList(
            _head.Neighbors,
            tile => (isAdvancing) ?
                   !tile.IsEdge()
                && (_deadEnds.FindIndex(item => item.Id == tile.Id) == -1)       // if cell is not listed in deadEnds list
                && (_wallsCache.FindIndex(item => item.Id == tile.Id) == -1)     // if cell is not listed in wallsCache list
                && (_pathHistory.FindIndex(item => item.Id == tile.Id) == -1)    // if cell is not listed in pathHistory list
                :
                   !tile.IsEdge()
                && tile.WallInstance == 1
                && (_deadEnds.FindIndex(item => item.Id == tile.Id) == -1)       // if cell is not listed in deadEnds list
                && (_pathHistory.FindIndex(item => item.Id == tile.Id) == -1)    // if cell is not listed in pathHistory list
        );

        

        if (IsDeadEnd(neighbors)) { Retreat(); return; }

        retreatCount = 0;
        isAdvancing = true;
        SurroundByWalls(_head);
        DetermineNextHead(neighbors);

        if (!_pathHistory.Contains(_head)) { _pathHistory.Add(_head); }
        Walk();
        return;

    }

    // ------------------------- Simple Action Functions

    private void Retreat()
    {
        retreatCount++;
        _deadEnds.Add(_head);
        SurroundByWalls(_head);
        // fall back (at first detection offset)
        if (retreatCount==1 && _pathHistory.Count>0)
            _pathHistory.RemoveAt(_pathHistory.Count - 1);

        // fall back (consecutive detection)
        _head = GetPreviousCell();
        if (_head == null) return;
        isAdvancing = false;
        _pathHistory.RemoveAt(_pathHistory.Count - 1);
        Walk();
    }


    private void SurroundByWalls(Tile tile)
    {
        List<Tile> neighbors = tile.Neighbors;

        for (int i = 0; i < neighbors.Count; i++)
        {
            Tile neighbor = neighbors[i];

            if (!IsHead(neighbor) && (_pathHistory.FindIndex(item => item.Id == neighbor.Id) == -1) && !neighbor.IsFloor())
            {
                neighbor.Type = "wall";
                neighbor.WallInstance++;
                _wallsCache.Add(neighbor);
            }
        }

    }

    // ------------------------- Simple Functions

    private Tile GetPreviousCell() {
        if (_pathHistory.Count == 0) return null;
        return _pathHistory[^1];
    }

    private bool IsDeadEnd(List<Tile> neighbors) { return neighbors.Count == 0; }

    private bool IsHead(Tile tile) { return tile.Id == _head.Id; }

    private void DetermineNextHead(List<Tile> n) { _head = n[Helper.GenerateRandomNumber(0, n.Count - 1)]; }


}
