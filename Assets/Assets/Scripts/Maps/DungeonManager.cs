using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    [SerializeField] private int _size;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Player _player;

    [SerializeField] private Sprite _doorSprite;
    [SerializeField] private Sprite _wallSprite;
    [SerializeField] private Sprite _floorSprite;

    private Tile[,] _grid;

    public Tile[,] Grid { get { return _grid; } }

    private int _rowCount;
    private int _colCount;

    private Tile _endPoint;
    private Walker[] _walkers;

    readonly private int _cellSize = 2;
    readonly private int _walkerCount = 2;

    // ------------ Setters and Getters

    public Player Player { get { return _player; } }


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
        if (this._size < 20) this._size = 20;
        if (this._size > 50) this._size = 50;
        this._rowCount = this._size;
        this._colCount = this._size;
        
        // Make empty grid array
        this._grid = new Tile[this._rowCount, this._colCount];

        // Fill the grid array
        for (int x = 0; x < this._rowCount; x++)
            for (int y = 0; y < this._colCount; y++)
            {
                Tile tile = Instantiate(_tilePrefab);
                tile.Init(x,y,this);
                if(Helper.IsEdgeOfGrid(x,y, this._rowCount, this._colCount))
                    tile.Type = "edge";
                else
                    tile.Type = "null";
                this._grid[x, y] = tile;
            }

    }
    private void NetworkNeighbors()
    {
        for (int x = 0; x < this._rowCount; x++)
            for (int y = 0; y < this._colCount; y++)
                this._grid[x, y].DetermineNeighbors();
    }

    private void DrawMaze()
    {

        Gizmos.color = Color.yellow;
        float xBase = -(this._rowCount) + (_cellSize / 2);
        float yBase = (this._colCount) - (_cellSize / 2);

        for (int y = 0; y < this._colCount; y++)
        {
            for (int x = 0; x < this._rowCount; x++)
            {

                Tile tile = _grid[x, y];

                Vector3 spawnPosition = new(xBase + (x * _cellSize), yBase - (y * _cellSize), 0);

                if (tile.IsWall() || tile.IsEdge())
                    tile.DrawTile(_wallSprite, spawnPosition, "wall");
                else if (tile.IsFloor() || tile.IsSpawnPoint())
                    tile.DrawTile(_floorSprite, spawnPosition, "floor");
                else if (tile.IsGoalPoint())
                    tile.DrawTile(_doorSprite, spawnPosition, "wall");

                if (tile.IsSpawnPoint())
                    this._player.transform.position = spawnPosition;
            }
        }

    }

    private void SpawnWalkers()
    {
        Walker[] walkers = new Walker[this._walkerCount];

        int[] x = Helper.GenerateRandomNumbersRange(1, this._rowCount - 2, this._walkerCount);
        int[] y = Helper.GenerateRandomNumbersRange(1, this._colCount - 2, this._walkerCount);

        for (int i = 0; i < this._walkerCount; i++)
            walkers[i] = new Walker(this._grid[x[i], y[i]], this);

        this._walkers = walkers;
    }

    private void DetermineEndPoint()
    {

        int n1 = Helper.GenerateRandomNumberEither(0, this._rowCount - 1);
        int n2 = Helper.GenerateRandomNumber(1, this._rowCount - 2);

        if (Helper.GenerateRandomBool())
        {
            int temp = n1;
            n1 = n2;
            n2 = temp;
        }

        this._endPoint = this._grid[n1, n2];
        this._endPoint.Type = "goal";
    }

    private void ActivateWalkers()
    {
        for (int i = 0; i < this._walkers.Length; i++)
            this._walkers[i].Walk();
    }


    public bool IsWithinGrid(int x, int y)
    {
        if(x > 0 && x < this._rowCount-1 && y > 0 && y < this._colCount-1)
            return true;
        return false;
    }

    private void ForceFloor(int x, int y, bool isPlayerSpawnPoint = false)
    {
        if(this.IsWithinGrid(x, y))
        {
            Tile tile = this._grid[x, y];

            if (isPlayerSpawnPoint)
                tile.Type = "spawn";
            else
                tile.Type = "floor";
        }
    }

    private void ExpandEndPoint()
    {
        (int x, int y) = this._endPoint.GridCoordinates;

        if (y == 0 || y == this._colCount - 1) this.ForceFloor(x + 0, y - 1, true);
        if (y == 0 || y == this._colCount - 1) this.ForceFloor(x + 0, y + 1, true);
        if (x == 0 || x == this._rowCount - 1) this.ForceFloor(x + 1, y + 0, true);
        if (x == 0 || x == this._rowCount - 1) this.ForceFloor(x - 1, y + 0, true);
        this.ForceFloor(x - 1, y + 1);
        this.ForceFloor(x + 1, y + 1);
        this.ForceFloor(x + 1, y - 1);
        this.ForceFloor(x - 1, y - 1);
    }

}