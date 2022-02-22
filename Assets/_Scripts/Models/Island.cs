using EventCallbacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    Dictionary<int, int> positionHeights;

    int width;
    int height;

    public int Width { get => width; private set => width = value; }
    public int Height { get => height; private set => height = value; }

    readonly Tile[,] Tiles;
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

    public void TryPlaceBuilding(Tile tile, BuildingBase buildingBase)
    {
        int x = tile.X;
        int y = tile.Y;

        if (ValidateBuildingPlacement(tile, buildingBase) == false) return;
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

    bool ValidateBuildingPlacement(Tile tile, BuildingBase buildingBase)
    {

        // Check if tile is occupied
        if (tile.CanBuild(out string debugMessage) == false)
        {
            Debug.Log(debugMessage);
            return false;
        }

        // Check if there's enough resources
        if (ResourceController.Instance.DoIHaveEnough(buildingBase.Cost) == false)
        {
            Debug.Log($"Can't place {buildingBase.name} at {tile.X},{tile.Y} because you don't have enough crypto");
            return false;
        }

        return true;
    }

    public void TryDestroyBuilding(Tile tile)
    {
        // Check whether there's a building in the tile
        if (!tile.IsOccupied)
        {
            Debug.Log($"Can't remove building at {tile.X},{tile.Y} because the tile is not occupied");
            return;
        }

        // Check whether there's a building on top
        Tile tileAbove = Tiles[tile.X, tile.Y + 1];
        if (tileAbove.IsOccupied)
        {
            Debug.Log($"Can't remove {tile.Building} at {tile.X},{tile.Y} because the tile above is occupied");
            return;
        }

        tileAbove.IsSupported = false;
        positionHeights[tile.X] = tile.Y + 1;

        tile.Building.Remove();
        tile.Building = null;
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        Building b = buildingEvent.building;
        buildings.Remove(b);
        positionHeights[b.X]--;
    }
}
