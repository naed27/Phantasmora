using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Fog _fogPrefab;
    [SerializeField] private HeatSignature _heatSignaturePrefab;

    private Fog _fog;
    private HeatSignature _heatSignature;

    private string _id;
    private string _type;

    private Coordinate _gridCoordinates;
    private DungeonManager _dungeonManager;
    private List<Tile> _neighbors = new();
    private int _wallInstance = 0;


    // ----------------------- setters and getters
    public string Id { get { return _id; } set { _id = value; } }

    public string Type { get { return _type; } set { _type = value; } }

    public int WallInstance { get { return _wallInstance; } set { _wallInstance = value; } }

    public List<Tile> Neighbors { get { return _neighbors; } set { _neighbors = value; } }

    public Coordinate GridCoordinates { get { return _gridCoordinates; } set { _gridCoordinates = value; } }


    // ----------------------- functions

    public void Init(int x, int y, DungeonManager dungeonManager)
    {
        SetId(x, y);
        SetCoordinates(x, y);
        SetDungeonManager(dungeonManager);

        SetName();
        SetParent(dungeonManager);
    }

    public void DetermineNeighbors()
    {
        (int x, int y) = this._gridCoordinates;
        if (this._dungeonManager.IsWithinGrid(x - 1, y)) _neighbors.Add(this._dungeonManager.Grid[x - 1, y]);
        if (this._dungeonManager.IsWithinGrid(x + 1, y)) _neighbors.Add(this._dungeonManager.Grid[x + 1, y]);
        if (this._dungeonManager.IsWithinGrid(x, y - 1)) _neighbors.Add(this._dungeonManager.Grid[x, y - 1]);
        if (this._dungeonManager.IsWithinGrid(x, y + 1)) _neighbors.Add(this._dungeonManager.Grid[x, y + 1]);
    }


    public bool IsEdge() { return this._type == "edge"; }

    public bool IsWall() { return this._type == "wall"; }

    public bool IsFloor() { return this._type == "floor"; }

    public bool IsNull() { return this._type == "null"; }

    public bool IsGoalPoint() { return this._type == "goal"; }

    public bool IsSpawnPoint() { return this._type == "spawn"; }

    public bool IsSameCell(Tile tile) { return tile != null && this._id == tile.Id; }

    public void DrawTile(Sprite sprite, Vector3 position, string layer)
    {
        SetLayer(layer);
        SetSprite(sprite);
        SetPosition(position);
        CreateFog(position);
        CreateHeatSignature(position, sprite);
    }



    // ----------------------- private functions


    private void SetId(int x, int y) { _id = "(" + x.ToString() + ", " + y.ToString() + ")"; }
    private void SetCoordinates(int x, int y) { _gridCoordinates = new Coordinate(x, y); }
    private void SetDungeonManager(DungeonManager dungeonManager) { _dungeonManager = dungeonManager; }
    private void SetName() { transform.gameObject.name = "Tile" + Id; }
    private void SetParent(DungeonManager dungeonManager) { transform.parent = dungeonManager.transform; }


    private void CreateFog(Vector3 position)
    {
        _fog = Instantiate(_fogPrefab);
        _fog.name = "Fog";
        _fog.transform.parent = transform;
        _fog.transform.position = position;
        _fog.Init(_dungeonManager.Player);
    }

    private void CreateHeatSignature(Vector3 position, Sprite sprite)
    {
        _heatSignaturePrefab = Instantiate(_heatSignaturePrefab);
        _heatSignaturePrefab.name = "Heat Signature";
        _heatSignaturePrefab.transform.parent = transform;
        _heatSignaturePrefab.transform.position = position;
        _heatSignaturePrefab.Init(sprite);
    }

    private void SetSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    private void SetLayer(string layer)
    {
        if (layer == "floor")
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            transform.gameObject.layer = LayerMask.NameToLayer("Floor");
        }
        else
            transform.gameObject.layer = LayerMask.NameToLayer("Wall");
    }


}
