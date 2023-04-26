using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Fog _fog;
    [SerializeField] private HeatSignature _heatSignature;
    [SerializeField] private TileInteractive _tileInteractive;

    private string _id;
    private string _type;

    private Coordinate _gridCoordinates;
    private DungeonManager _dungeonManager;
    private List<Tile> _neighbors = new();
    private int _wallInstance = 0;

    private Vector3 _position;

    public Vector3 Position { get { return _position; } }


    // ----------------------- setters and getters
    public string Id { get { return _id; } set { _id = value; } }

    public string Type { get { return _type; } set { _type = value; } }

    public int WallInstance { get { return _wallInstance; } set { _wallInstance = value; } }

    public List<Tile> Neighbors { get { return _neighbors; } set { _neighbors = value; } }

    public Coordinate GridCoordinates { get { return _gridCoordinates; } set { _gridCoordinates = value; } }


    //------------

   

    // ----------------------- functions

    public void Init(int x, int y, DungeonManager dungeonManager)
    {
        SetId(x, y);
        SetName();
        SetCoordinates(x, y);
        SetParent(dungeonManager); 
        _dungeonManager = dungeonManager;
        _tileInteractive.Init(this, dungeonManager.Player, dungeonManager.Player.InteractKey);
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

    public bool IsFloor() { return this._type == "floor" || this._type == "spawn"; }

    public bool IsNull() { return this._type == "null"; }

    public bool IsGoalPoint() { return this._type == "goal"; }

    public bool IsSpawnPoint() { return this._type == "spawn"; }

    public bool IsSameCell(Tile tile) { return tile != null && this._id == tile.Id; }

    public void DrawTile(Sprite sprite, Material material, Vector3 position, string layer)
    {
        SetLayer(layer);
        SetSpriteAndMaterial(sprite, material);
        SetPosition(position);
        _fog.Init(_dungeonManager.Player);
        _heatSignature.Init(sprite);
    }
    // -------------

    private void OnTriggerStay2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.CompareTag("Guard"))
            if (hitObject.TryGetComponent(out Guard guard))
                guard.SetNewGoalFrom(this);
    }


    // ----------------------- private functions


    private void SetId(int x, int y) { _id = "(" + x.ToString() + ", " + y.ToString() + ")"; }
    private void SetCoordinates(int x, int y) { _gridCoordinates = new Coordinate(x, y); }
    private void SetName() { transform.gameObject.name = "Tile" + Id; }
    private void SetParent(DungeonManager dungeonManager) { transform.parent = dungeonManager.transform; }


    private void SetSpriteAndMaterial(Sprite sprite, Material material)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.material = material;
    }
    private void SetPosition(Vector3 position)
    {
        _position = position;
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
