using OrcaDefense.Models;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
    public Island Island;

    public void TryPlaceBuilding(Tile tile, BuildingType buildingType)
    {
        ValidateBuildingPlacement(tile, buildingType);
        PlaceBuilding(tile, buildingType);
    }

    void ValidateBuildingPlacement(Tile tile, BuildingType buildingType)
    {
        if (ResourceManager.Instance.CheckResourcesAvailability(buildingType.Cost) == false)
        {
            Debug.Log($"Can't place {buildingType} in {tile} because you don't have enough resources");
            return;
        }

        if (tile.CanBuild() == false)
        {
            Debug.Log($"Can't place {buildingType} in {tile} because it's not available for building");
            return;
        }
    }

    void PlaceBuilding(Tile tile, BuildingType BuildingType)
    {
        Island.Build(tile, BuildingType);
    }

    public void TryDestroyBuilding(Tile tile)
    {
        Tile tileAbove = Island.Tiles[tile.X, tile.Y];
        ValidateBuildingDestruction(tile, tileAbove);
        DestroyBuilding(tile, tileAbove);
    }

    private void ValidateBuildingDestruction(Tile tile, Tile tileAbove)
    {
        // Check whether there's a building in the tile
        if (!tile.IsOccupied)
        {
            Debug.Log($"There's no building in {tile} to destroy");
            return;
        }

        // Check whether there's a building on top
        if (tileAbove.IsOccupied)
        {
            Debug.Log($"Can't remove {tile.Building} at {tile} because the tile above is occupied");
            return;
        }
    }

    void DestroyBuilding(Tile tile, Tile tileAbove)
    {
        tileAbove.IsSupported = false; // TODO: remove from action manager
        Island.positionHeights[tile.X] = tile.Y + 1;

        tile.Building.Remove();
        tile.Building = null;
    }
}
