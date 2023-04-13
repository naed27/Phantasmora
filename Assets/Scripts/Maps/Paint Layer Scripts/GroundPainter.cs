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
    GridMap gridMap;

    [SerializeField] int groundRowCount;
    [SerializeField] int groundColCount;

    // Start is called before the first frame update
    void Start()
    {
        gridMap = GetComponent<GridMap>();
        gridMap.Initialize(groundRowCount, groundColCount);
        gridMap.DrawMaze();
    }

}
