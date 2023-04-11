using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width, height;
    public GameObject wallPrefab, floorPrefab;

    private bool[,] grid;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        grid = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = Random.value < 0.5f;
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y])
                {
                    Instantiate(wallPrefab, new Vector3(x, 0f, y), Quaternion.identity);
                }
                else
                {
                    Instantiate(floorPrefab, new Vector3(x, 0f, y), Quaternion.identity);
                }
            }
        }
    }
}
