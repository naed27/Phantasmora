using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public int rowCount;
    public int colCount;

    bool[,] grid;

    public void Init(int length, int height)
    {
        grid = new bool[length, height];
        this.rowCount = length;
        this.colCount = height;
    }

    public void Set(int x, int y, bool display)
    {
        if (!CheckPosition(x, y))
        {
            return;
        }

        grid[x, y] = display;    
    }

    public bool Get(int x, int y)
    {
        if (!CheckPosition(x, y))
        {
            return false;
        }
        return grid[x, y];
    }

    public bool CheckPosition(int x, int y)
    {

        if(x < 0 || x >= rowCount || x < 0 || y >= colCount)
        {
            Debug.LogWarning("Coordinates out of bounds.");
            return false;
        }

        return true;
    }
}
