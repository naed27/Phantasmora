using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Requirements for script to run properly

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]

public class GroundPainter : MonoBehaviour
{
    // Properties
    Tilemap tileMap;
    GridMap gridMap;
    TilemapRenderer tilemapRenderer;

    [SerializeField] TileBase ground1;
    [SerializeField] TileBase ground2;

    [SerializeField] int gridRowCount;
    [SerializeField] int gridColCount;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            (gridRowCount * -1), // to center x
            (gridColCount * -1), // to center y
            (transform.position.z) // to keep z
        );

        tileMap = GetComponent<Tilemap>();
        gridMap = GetComponent<GridMap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        gridMap.Init(gridRowCount, gridColCount);
        UpdateTileMap();
        tilemapRenderer.sortingOrder = 7;

    }

    // Update is called once per frame
    void UpdateTileMap()
    {
        for (int x = 0; x < gridMap.rowCount; x++)
        {
            for (int y = 0; y < gridMap.colCount; y++)
            {
                PutGrass(x, y);
            }
        }
    }

    void PutGrass(int x, int y)
    {
        bool randomBool = new System.Random().Next(2) == 0;

        if (randomBool == true)
        {
            tileMap.SetTile(new Vector3Int(x, y, 0), ground1);
        }
        else
        {
            tileMap.SetTile(new Vector3Int(x, y, 0), ground2);
        }
    }
}
