using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    // ---------------- UI

    [SerializeField] private GameObject _mainMenu;


    //---------------- Properties

    [SerializeField] private int _size;
    [SerializeField] private int _guardCount;
    [SerializeField] private int _dungeonKeyCount;

    [SerializeField] private Player _player;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Guard _guardPrefab;
    [SerializeField] private DungeonKey _dungeonKeyPrefab;

    [SerializeField] private Sprite _doorSprite;
    [SerializeField] private Sprite _wallSprite;
    [SerializeField] private Sprite _floorSprite;

    [SerializeField] private Material _wallMaterial;
    [SerializeField] private Material _floorMaterial;
    [SerializeField] private Material _doorMaterial;

    private Tile[,] _grid = new Tile[0, 0];

    private int _rowCount;
    private int _colCount;

    private Tile _goalPoint;

    private List<Guard> _guards = new();
    private List<Walker> _walkers = new();
    private List<DungeonKey> _dungeonKeys = new ();

    private List<Tile> _floorTiles;
    private List<Tile> _spawnableTiles;

    readonly private int _cellSize = 2;
    readonly private int _walkerCount = 2;

    // ------------ Setters and Getters

    public Tile[,] Grid { get { return _grid; } }

    public Player Player { get { return _player; } }
    public List<DungeonKey> DungeonKeys => _dungeonKeys;


    public void Init()
    {
        _mainMenu.SetActive(false);
        _player.Init();

        DestroyDungeon();

        SetupMazeCells();
        NetworkNeighbors();
        DetermineEndPoint();
        SpawnWalkers();
        ActivateWalkers();
        ExpandEndPoint();
        DrawMaze();
        InitializeSpawnableTiles();
        SpawnGuards();
        SpawnDungeonKeys();

        _player.ShowStatusBar();
    }

    private void SetupMazeCells()
    {
        // Setup values for properties
        if (_size < 20) _size = 20;
        if (_size > 50) _size = 50;
        _rowCount = _size;
        _colCount = _size;

        _grid = new Tile[_rowCount, _colCount];

        // Fill the grid array
        for (int x = 0; x < _rowCount; x++)
            for (int y = 0; y < _colCount; y++)
            {
                Tile tile = Instantiate(_tilePrefab);
                tile.Init(x,y,this);
                if(Helper.IsEdgeOfGrid(x,y, _rowCount, _colCount))
                    tile.Type = "edge";
                else
                    tile.Type = "null";
                _grid[x, y] = tile;
            }

    }
    private void NetworkNeighbors()
    {
        for (int x = 0; x < _rowCount; x++)
            for (int y = 0; y < _colCount; y++)
                _grid[x, y].DetermineNeighbors();
    }

    private void DrawMaze()
    {

        _floorTiles = new List<Tile>();

        Gizmos.color = Color.yellow;
        float xBase = -(_rowCount) + (_cellSize / 2);
        float yBase = (_colCount) - (_cellSize / 2);

        for (int y = 0; y < _colCount; y++)
        {
            for (int x = 0; x < _rowCount; x++)
            {

                Tile tile = _grid[x, y];

                Vector3 spawnPosition = new(xBase + (x * _cellSize), yBase - (y * _cellSize), 0);

                if (tile.IsWall() || tile.IsEdge() || tile.IsNull())
                    tile.DrawTile(_wallSprite, _wallMaterial, spawnPosition, "wall");
                else if (tile.IsFloor() || tile.IsSpawnPoint())
                {
                    tile.DrawTile(_floorSprite, _floorMaterial, spawnPosition, "floor");
                    _floorTiles.Add(tile);
                }
                else if (tile.IsGoalPoint())
                    tile.DrawTile(_doorSprite, _doorMaterial, spawnPosition, "wall");

                if (tile.IsSpawnPoint())
                    _player.transform.position = spawnPosition;
            }
        }

    }

    private void SpawnWalkers()
    {
        _walkers = new List<Walker>();

        int[] x = Helper.GenerateRandomNumbersRange(1, _rowCount - 2, _walkerCount);
        int[] y = Helper.GenerateRandomNumbersRange(1, _colCount - 2, _walkerCount);

        for (int i = 0; i < _walkerCount; i++)
            _walkers.Add(new Walker(_grid[x[i], y[i]], this));

    }

    private void InitializeSpawnableTiles()
    {
        _spawnableTiles = new List<Tile>();

        (int x, int y) = _goalPoint.GridCoordinates;
        _spawnableTiles = Helper.FilterList(
            _floorTiles,
            tile =>
                Helper.AreNumbersXDistanceApart(tile.GridCoordinates.X, x, _size / 2)
            || Helper.AreNumbersXDistanceApart(tile.GridCoordinates.Y, y, _size / 2)
        );
    }

    private void SpawnGuards()
    {
        _guards = new List<Guard>();

        for (int i = 0; i < _guardCount; i++)
        {
            Tile tile = _spawnableTiles[Helper.GenerateRandomNumber(0, _spawnableTiles.Count - 1)];
            _guards.Add(Instantiate(_guardPrefab));
            _guards[i].Init(tile, this);
        }
    }

    private void SpawnDungeonKeys()
    {
        _dungeonKeys = new List<DungeonKey>();

        for (int i = 0; i < _dungeonKeyCount; i++)
        {
            Tile tile = _spawnableTiles[Helper.GenerateRandomNumber(0, _spawnableTiles.Count - 1)];
            _dungeonKeys.Add(Instantiate(_dungeonKeyPrefab));
            _dungeonKeys[i].Init(i,tile, this);
        }
    }



    private void DetermineEndPoint()
    {

        int n1 = Helper.GenerateRandomNumberEither(0, _rowCount - 1);
        int n2 = Helper.GenerateRandomNumber(1, _rowCount - 2);

        if (Helper.GenerateRandomBool())
        {
            int temp = n1;
            n1 = n2;
            n2 = temp;
        }

        _goalPoint = _grid[n1, n2];
        _goalPoint.Type = "goal";
    }

    private void ActivateWalkers()
    {
        for (int i = 0; i < _walkers.Count; i++)
            _walkers[i].Walk();
    }


    public bool IsWithinGrid(int x, int y)
    {
        if(x > 0 && x < _rowCount-1 && y > 0 && y < _colCount-1)
            return true;
        return false;
    }

    private void ForceFloor(int x, int y, bool isPlayerSpawnPoint = false)
    {
        if(IsWithinGrid(x, y))
        {
            Tile tile = _grid[x, y];

            if (isPlayerSpawnPoint)
                tile.Type = "spawn";
            else
                tile.Type = "floor";
        }
    }

    private void ExpandEndPoint()
    {
        (int x, int y) = _goalPoint.GridCoordinates;

        if (y == 0 || y == _colCount - 1) ForceFloor(x + 0, y - 1, true);
        if (y == 0 || y == _colCount - 1) ForceFloor(x + 0, y + 1, true);
        if (x == 0 || x == _rowCount - 1) ForceFloor(x + 1, y + 0, true);
        if (x == 0 || x == _rowCount - 1) ForceFloor(x - 1, y + 0, true);
        ForceFloor(x - 1, y + 1);
        ForceFloor(x + 1, y + 1);
        ForceFloor(x + 1, y - 1);
        ForceFloor(x - 1, y - 1);
    }

    private void DestroyTiles()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (_grid[x, y])
                    Destroy(_grid[x, y].transform.gameObject);
            }
        }

        _grid = new Tile[_rowCount, _colCount];
    }

    private void DestroyGuards()
    {
        for (int i = 0; i < _guards.Count; i++)
            if (_guards[i])
                Destroy(_guards[i].transform.gameObject);
        _guards = new List<Guard>();
    }


    private void DestroyDungeonKeys()
    {
        for (int i = 0; i < _dungeonKeys.Count; i++)
            if(_dungeonKeys[i])
                Destroy(_dungeonKeys[i].transform.gameObject);
        _dungeonKeys = new List<DungeonKey>();
    }

    public void DestroyDungeon()
    {
        DestroyGuards();
        DestroyTiles();
        DestroyDungeonKeys();
    }
}