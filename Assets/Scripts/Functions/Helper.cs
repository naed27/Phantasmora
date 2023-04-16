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

    public static bool GenerateRandomBool() { return UnityEngine.Random.Range(0, 2) != 0; }


    public static Cell[] FilterArray(Cell[] array, Func<Cell, bool> condition)
    {
        return array.Where(x => condition(x)).ToArray();
    }

    public static List<T> FilterList<T>(List<T> inputList, Func<T, bool> condition)
    {
        List<T> outputList = new ();
        foreach (T item in inputList)
        {
            if (condition(item))
            {
                outputList.Add(item);
            }
        }
        return outputList;
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

    public static bool IsElementInArray<T>(T[] array, T element)
    {
        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(element))
                return true;

        return false;
    }


    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360

        float angleRadius = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
    }
}

