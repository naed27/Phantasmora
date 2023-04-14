using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class Helper
{
    public static int GenerateRandomNumber(int minValue, int maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue + 1);
    }

    public static int[] GenerateRandomNumbersRange(int rangeStart, int rangeEnd, int numberOfRandoms)
    {
        int[] randomNumbers = new int[numberOfRandoms];
        System.Random random = new();
        for (int i = 0; i < randomNumbers.Length; i++)
        {
            randomNumbers[i] = random.Next(rangeStart, rangeEnd);
        }
        return randomNumbers;
    }

    public static bool IsEdgeOfGrid(int x, int y, int gridWidth, int gridHeight)
    {
        bool isOnEdge = false;

        if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1)
        {
            isOnEdge = true;
        }

        return isOnEdge;
    }

    public static int GenerateRandomNumberEither(int a, int b)
    {
        float randomNumber = UnityEngine.Random.value;
        if (randomNumber < 0.5f)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    public static bool GenerateRandomBool()
    {
        int randomInt = UnityEngine.Random.Range(0, 2);
        return (randomInt == 0) ? false : true;
    }


    public static Cell[] FilterGrid(Cell[] array, Func<Cell, bool> condition)
    {
        return array.Where(x => condition(x)).ToArray();
    }

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }
}

