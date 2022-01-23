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

    public Island(int width = 100, int height = 20)
    {
        Width = width;
        Height = height;
        positionHeights = new Dictionary<int, int>();
        buildings = new List<Building>();

        Tiles = new Tile[100, 20];
        for (int x = 0; x < width; x++)
        {
            positionHeights[x] = 0;

            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y);
            }
        }
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

    public Tile GetHighestFreeTileAtCoords(Vector3 coords)
    {
        int x = Mathf.RoundToInt(coords.x);
        if (!positionHeights.ContainsKey(x)) return null;
        int y = positionHeights[x];
        return Tiles[x, y];
    }

    public void PlaceBuilding(int x, int y, BuildingBase buildingBase)
    {
        Tile tile = Tiles[x, y];
        if (ValidateBuildingPlacement(tile, buildingBase) == false) return;
        tile.Building = new Building(x, y, buildingBase);
        Tiles[x, y + 1].IsSupported = true;
        positionHeights[x] = y + 1;

        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
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

    void OnBuildingRemoved(BuildingEvent buildingEvent)
    {
        buildings.Remove(buildingEvent.building);
    }
}
