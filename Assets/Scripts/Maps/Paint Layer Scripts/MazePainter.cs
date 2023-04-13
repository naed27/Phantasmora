using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Requirements for script to run properly

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]

public class MazePainter : MonoBehaviour
{
    // Properties
    Tilemap tileMap;
    GridMap gridMap;
    TilemapRenderer tilemapRenderer;

    [SerializeField] TileBase wall;
    [SerializeField] TileBase floor1;
    [SerializeField] TileBase floor2;

    [SerializeField] int mazeRowCount;
    [SerializeField] int mazeColCount;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            (mazeRowCount * -1), // to center x
            (mazeColCount * -1), // to center y
            (transform.position.z) // to keep z
        );

        tileMap = GetComponent<Tilemap>();
        gridMap = GetComponent<GridMap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        gridMap.Initialize(mazeRowCount, mazeColCount);
        DrawMaze();
        tilemapRenderer.sortingOrder = 8;
    }

    // Update is called once per frame
    void DrawMaze()
    {
        this.DrawOuterWalls(gridMap);
        this.DrawInnerWalls(gridMap);
    }

    void DrawOuterWalls(GridMap gridMap)
    {
        for(int i=0; i < gridMap.colCount; i++)
        {
            tileMap.SetTile(new Vector3Int(0, i, 0), wall);
            tileMap.SetTile(new Vector3Int(i, 0, 0), wall);
        }

        for (int j = gridMap.colCount-1; j > 0; j--)
        {
            tileMap.SetTile(new Vector3Int(gridMap.colCount - 1, j, 0), wall);
            tileMap.SetTile(new Vector3Int(j, gridMap.colCount - 1, 0), wall);
        }
    }

    void DrawInnerWalls(GridMap gridMap)
    {
        DetermineStartPoints();
    }

    void DetermineStartPoints()
    {

    }

    void PutFloor(int x, int y)
    {
        bool randomBool = new System.Random().Next(2) == 0;

        if (randomBool == true)
        {
            tileMap.SetTile(new Vector3Int(x, y, 0), floor1);
        }
        else
        {
            tileMap.SetTile(new Vector3Int(x, y, 0), wall);
        }
    }

}



