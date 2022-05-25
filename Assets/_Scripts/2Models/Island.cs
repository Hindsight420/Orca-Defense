using EventCallbacks;
using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Island
{
    public Dictionary<int, int> positionHeights;

    int width;
    int height;

    public int Width { get => width; private set => width = value; }
    public int Height { get => height; private set => height = value; }

    public readonly Tile[,] Tiles;

    public Island(int width, int height)
    {
        Width = width;
        Height = height;
        positionHeights = new Dictionary<int, int>();

        Tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            positionHeights[x] = 0;

            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y);
            }
        }

        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
    }

    public Tile GetTileAtCoords(Vector2 coords)
    {
        int x = Mathf.FloorToInt(coords.x);
        int y = Mathf.FloorToInt(coords.y);

        return GetTileAtCoords(x, y);
    }

    public Tile GetTileAtCoords(int x, int y)
    {
        try
        {
            return Tiles[x, y];
        }
        catch (IndexOutOfRangeException)
        {
            // Debug.Log($"There's no tile here {coords}");
            return null;
        }
    }

    public void Build(Tile tile, BuildingType buildingType)
    {
        int x = tile.X;
        int y = tile.Y;

        positionHeights[x] = y + 1;
        Building b = new(buildingType, tile);

        new BuildingCreatedEvent().FireEvent(b);
    }

    public Tile GetHighestFreeTileAt(Vector3 coords)
    {
        int x = Mathf.RoundToInt(coords.x);
        return GetHighestFreeTileAt(x);
    }

    public Tile GetHighestFreeTileAt(int x)
    {
        if (!positionHeights.ContainsKey(x)) return null;
        int y = positionHeights[x];
        return Tiles[x, y];
    }

    public Tile[] GetAdjacentTiles(int x, int y)
    {
        var nullsIncluded = new Tile[] { Right(x, y), Left(x, y), Up(x, y), Down(x, y) };
        return nullsIncluded.Where(t => t != null).ToArray();
    }

    public Tile Right(int x, int y)
    {
        return x < width - 1 ? Tiles[++x, y] : null;
    }

    public Tile Left(int x, int y)
    {
        return x > 0 ? Tiles[--x, y] : null;
    }

    public Tile Up(int x, int y)
    {
        return y < height - 1 ? Tiles[x, ++y] : null;
    }

    public Tile Down(int x, int y)
    {
        return y > 0 ? Tiles[x, --y] : null;
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        positionHeights[b.X]--;
    }
}
