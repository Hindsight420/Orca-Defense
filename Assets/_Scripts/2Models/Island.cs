using EventCallbacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    public Dictionary<int, int> positionHeights;

    int width;
    int height;

    public int Width { get => width; private set => width = value; }
    public int Height { get => height; private set => height = value; }

    public readonly Tile[,] Tiles;
    public List<Building> buildings;
    public Action<Building> cbOnBuildingCreated;

    public Island(int width, int height)
    {
        Width = width;
        Height = height;
        positionHeights = new Dictionary<int, int>();
        buildings = new List<Building>();

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

    public Tile GetTileAtCoords(Vector3 coords)
    {
        int x = Mathf.RoundToInt(coords.x);
        int y = Mathf.RoundToInt(coords.y);
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

    public void Build(Tile tile, BuildingBase buildingBase)
    {
        int x = tile.X;
        int y = tile.Y;

        positionHeights[x] = y + 1;
        tile.Building = new Building(x, y, buildingBase);

        try
        {
            Tiles[x, y + 1].IsSupported = true;
        }
        catch (IndexOutOfRangeException)
        {
            positionHeights.Remove(x);
            Debug.Log($"There is no tile above {x}, {y} to support");
        }
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

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.Building;
        buildings.Remove(b);
        positionHeights[b.X]--;
    }
}
