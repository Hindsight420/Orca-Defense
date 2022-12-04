using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandManager : Singleton<IslandManager>
{
    public int Width;
    public int Height;

    public Island Island;
    [SerializeField] IslandView view;
    private Logger Logger { get => Logger.Instance; }

    private List<Building> buildingsToConstruct = new();
    private int startTick;
    private const int ticksPerResource = 5; // time between adding resources to buildings

    void Start()
    {
        Island = new Island(Width, Height);
        view.Island = Island;

        BuildingCreatedEvent.RegisterListener(OnBuildingCreated);
        BuildingChangedEvent.RegisterListener(OnBuildingChanged);
        BuildingRemovedEvent.RegisterListener(OnBuildingRemoved);
    }

    void OnBuildingCreated(BuildingCreatedEvent buildingEvent)
    {
        view.OnBuildingCreated(buildingEvent);
    }

    void OnBuildingChanged(BuildingChangedEvent buildingEvent)
    {
        view.OnBuildingChanged(buildingEvent);
    }

    void OnBuildingRemoved(BuildingRemovedEvent buildingEvent)
    {
        view.OnBuildingRemoved(buildingEvent);
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
        Building b = Island.CreateBuilding(tile, BuildingType);

        ConstructBuildingOverTime(b);
    }

    // Temporary way of constructing buildings
    // We should employ penguins to do this later
    private void ConstructBuildingOverTime(Building b)
    {
        buildingsToConstruct.Add(b);

        TimeTicker.OnTick += OnTick;
        startTick = TimeTicker.CurrentTick;
    }

    private void OnTick(object sender, int e)
    {
        if (TimeTicker.GetInnerTick(startTick) % ticksPerResource == 0)
        {
            foreach (Building b in buildingsToConstruct)
            {
                AddResourcesToBuilding(b);
            }
        }
    }

    private void AddResourcesToBuilding(Building b)
    {
        // If all resources are there, construct the building
        if (b.RemainingResources.Count == 0)
        {
            b.Construct();
            buildingsToConstruct.Remove(b);
            return;
        }

        // Check which resources to add to the building
        List<ResourceValue> resourcesToAdd = new();
        foreach (ResourceValue resource in b.RemainingResources)
        {
            resourcesToAdd.Add(new(resource.Type, 1));
        }

        b.AddResources(resourcesToAdd);
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
        Building b = tile.Building;
        tile.Building = null;
        tile.Validator = new BaseBuildingValidator(tile);

        new BuildingRemovedEvent().FireEvent(b);
    }
}
