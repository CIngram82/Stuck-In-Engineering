using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridMap<T>
{

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private Vector3 originPos;
    public T[,] gridArray;
    /// <summary>
    /// Creates the grid map with no items in the grid array
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="originPos"></param>
    public GridMap(int width, int height,  Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.originPos = originPos;

        gridArray = new T[width, height];

    }
    /// <summary>
    /// This Creates the grid map and fills in the grid with the object passed in
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="originPos"></param>
    /// <param name="createGridObject"></param>
    public GridMap(int width, int height,  Vector3 originPos, Func<GridMap<T>,int,int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.originPos = originPos;

        gridArray = new T[width, height];


        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x,y);
            }
        }

    }

    public int Height
    {
        get { return height; }
    }
    public int Width
    {
        get { return width; }
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) + originPos;
    }
    private Vector2Int GetXY(Vector3 worldPosition)
    {
        return new Vector2Int
        {
            x = Mathf.FloorToInt((worldPosition.x - originPos.x)),
            y = Mathf.FloorToInt((worldPosition.y - originPos.y))
        };
    }

    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = value;
        }
        else
        {
            Debug.LogError("Replacment out of range");
        }
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        Vector2Int posXY = GetXY(worldPosition);
        SetValue(posXY.x, posXY.y, value);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public T GetObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default;
        }
    }

    public T GetObject(Vector3 worldPosition)
    {
        Vector2Int posXY = GetXY(worldPosition);
        return GetObject(posXY.x, posXY.y);
    }
}
