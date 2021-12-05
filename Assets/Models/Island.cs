using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    Tile[,] tiles;
    int width;
    int height;

    public int Width { get => width; private set => width = value; }
    public int Height { get => height; private set => height = value; }

    public Island(int width = 100, int height = 20)
    {
        Width = width;
        Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(x, y);
            }
        }
    }
}
