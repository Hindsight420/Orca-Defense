using EventCallbacks;
using UnityEngine;

public class IslandManager : Singleton<IslandManager>
{
    public int Width;
    public int Height;

    public Island Island;
    [SerializeField] IslandView view;

    void Start()
    {
        Island = new Island(Width, Height);
        view.Island = Island;

        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        view.OnBuildingRemoved(buildingEvent);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        view.OnBuildingCreated(buildingEvent);
    }

    public void TryPlaceBuilding(Tile tile, BuildingType buildingType)
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

        PlaceBuilding(tile, buildingType);
    }

    void PlaceBuilding(Tile tile, BuildingType BuildingType)
    {
        Island.Build(tile, BuildingType);
    }

    public void TryDestroyBuilding(Tile tile)
    {
        // Check whether there's a building in the tile
        if (!tile.IsOccupied)
        {
            Debug.Log($"There's no building in {tile} to destroy");
            return;
        }

        // Check whether there's a building on top
        Tile tileAbove = Island.Tiles[tile.X, tile.Y + 1];
        if (tileAbove.IsOccupied)
        {
            Debug.Log($"Can't remove {tile.Building} at {tile} because the tile above is occupied");
            return;
        }

        DestroyBuilding(tile, tileAbove);
    }

    void DestroyBuilding(Tile tile, Tile tileAbove)
    {
        tileAbove.IsSupported = false;
        Island.positionHeights[tile.X] = tile.Y + 1;

        tile.Building.Remove();
        tile.Building = null;
    }
}
