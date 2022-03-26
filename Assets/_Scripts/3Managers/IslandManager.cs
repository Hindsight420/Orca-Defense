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

        // Instantiate any buildings that already exist (from loading an existing save)
        foreach (Building building in Island.buildings)
        {
            new BuildingCreatedEvent().FireEvent(building);
        }
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        view.OnBuildingRemoved(buildingEvent);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        view.OnBuildingCreated(buildingEvent);
    }

    public void TryPlaceBuilding(Tile tile, BuildingBase buildingBase)
    {
        if (ResourceManager.Instance.CheckResourcesAvailability(buildingBase.Cost) == false)
        {
            Debug.Log($"Can't place {buildingBase.name} at {tile.X},{tile.Y} because you don't have enough crypto");
            return;
        }

        if (tile.CanBuild(out string debugMessage) == false)
        {
            Debug.Log(debugMessage);
            return;
        }

        PlaceBuilding(tile, buildingBase);
    }

    void PlaceBuilding(Tile tile, BuildingBase buildingBase)
    {
        Island.Build(tile, buildingBase);
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
        Tile tileAbove = Island.Tiles[tile.X, tile.Y + 1];
        if (tileAbove.IsOccupied)
        {
            Debug.Log($"Can't remove {tile.Building} at {tile.X},{tile.Y} because the tile above is occupied");
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
