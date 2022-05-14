using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandManager : Singleton<IslandManager>
{
    public int Width;
    public int Height;

    public Island Island;
    [SerializeField] IslandView view;
    [SerializeField] Logger Logger;

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
        //TODO: This needs to be moved at a later point.
        var validator = buildingType.GetBuildingValidator(tile);
        var buildingErrors = validator.ValidateResources(buildingType);
        if (buildingErrors.Any()) { Logger.LogMessages(buildingErrors, Logger.LogType.Error); return; }

        buildingErrors = validator.ValidateBuildingPosition(Island, buildingType.BuildingEnum);
        if (buildingErrors.Any()) { Logger.LogMessages(buildingErrors, Logger.LogType.Error); return; }

        PlaceBuilding(tile, buildingType, validator);
    }

    void PlaceBuilding(Tile tile, BuildingType BuildingType, IBuildingValidator newValidator)
    {
        tile.Validator = newValidator;
        Island.Build(tile, BuildingType);
    }

    public void TryDestroyBuilding(Tile tile)
    {
        if (tile.Building is null) { Logger.LogMessage("Nothing to destroy!", Logger.LogType.Error); return; }
        // Check whether there's a building in the tile
        var errors = tile.Validator.ValidateDestroyable(Island);
        if (errors.Any()) { Logger.LogMessages(errors, Logger.LogType.Error); return; }

        DestroyBuilding(tile);
    }

    void DestroyBuilding(Tile tile)
    {
        tile.Building.Remove();
        tile.Building = null;
        tile.Validator = new BaseBuildingValidator(tile);
    }
}
