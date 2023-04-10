using Assets._Scripts._3Managers;
using EventCallbacks;
using OrcaDefense.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandManager : Singleton<IslandManager>
{
    [SerializeField]
    private IslandView view;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private Logger _logger;
    private readonly List<Building> _buildingsToConstruct = new();
    private const int TICKS_PER_RESOURCE = 5; // time between adding resources to buildings
    private int _startTick;

    public Island Island { get; private set; }

    void Start()
    {
        Island = new Island(_width, _height);
        view.Island = Island;
        _logger = Logger.Instance;

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
        if (buildingErrors.Any()) { _logger.LogMessages(buildingErrors, Logger.LogType.Error); return; }

        buildingErrors = validator.ValidateBuildingPosition(Island, buildingType.BuildingEnum);
        if (buildingErrors.Any()) { _logger.LogMessages(buildingErrors, Logger.LogType.Error); return; }

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
        _buildingsToConstruct.Add(b);

        TimeTicker.OnTick += OnTick;
        _startTick = TimeTicker.CurrentTick;
    }

    private void OnTick(object sender, int e)
    {
        if (TimeTicker.GetInnerTick(_startTick) % TICKS_PER_RESOURCE == 0)
        {
            foreach (Building b in _buildingsToConstruct)
            {
                AddResourcesToBuilding(b);
            }

            _buildingsToConstruct.RemoveAll(b => b.State != BuildingState.Planned);
        }
    }

    private void AddResourcesToBuilding(Building b)
    {
        // If all resources are there, construct the building
        if (b.RemainingResources.Count == 0)
        {
            b.Construct();
            return;
        }

        // Check which resources to add to the building
        ResourceList resourcesToAdd = new();
        foreach (Resource resource in b.RemainingResources.Resources)
        {
            resourcesToAdd.Add(new Resource(resource.Type, 1));
        }

        b.AddResources(resourcesToAdd);
    }

    public void TryDestroyBuilding(Tile tile)
    {
        if (tile.Building is null) { _logger.LogMessage("Nothing to destroy!", Logger.LogType.Error); return; }
        // Check whether there's a building in the tile
        var errors = tile.Validator.ValidateDestroyable(Island);
        if (errors.Any()) { _logger.LogMessages(errors, Logger.LogType.Error); return; }

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
