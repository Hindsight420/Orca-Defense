using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    Dictionary<int, int> positionHeights;

    int width;
    public int Width { get => width; private set => width = value; }

    public Island(int width = 100)
    {
        Width = width;
        positionHeights = new Dictionary<int, int>();
        for (int x = 0; x < width; x++)
        {
            positionHeights[x] = 0;
        }
    }

    public Vector3 GetHighestPositionAtCoords(Vector3 coords)
    {
        int x = Mathf.RoundToInt(coords.x);
        if (!positionHeights.ContainsKey(x)) return new Vector3(-1, -1, 0);

        int y = positionHeights[x];
        return new Vector3(x, y, 0);
    }

    public void PlaceBuilding(Vector3 selectedPosition)
    {
        Building building = new Building((int)selectedPosition.x, (int)selectedPosition.y, "Square");

    }
}
