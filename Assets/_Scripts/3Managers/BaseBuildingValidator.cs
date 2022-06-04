using OrcaDefense.Models;
using System;
using System.Collections.Generic;

public class BaseBuildingValidator : IBuildingValidator
{
    protected readonly Tile _tile;
    public BaseBuildingValidator(Tile tile)
    {
        _tile = tile;
    }

    public List<string> ValidateBuildingPosition(Island map, BuildingTypeEnum buildingType)
    {
        var errors = new List<string>();
        errors.AddRange(ValidateTile(map));
        errors.AddRange(ValidateSupport(map));
        errors.AddRange(ValidateAdjacencies(map));
        errors.AddRange(ValidateAdjacency(buildingType));

        return errors;
    }

    //Does this building care if we build X building next to it?
    public virtual List<string> ValidateAdjacency(BuildingTypeEnum buildingToBuild)
    {
        return new List<string>();
    }

    //Does this building care if we build it next to it's neighbours?
    public virtual List<string> ValidateAdjacencies(Island map)
    {
        return new List<string>();
    }

    //Can the building be placed in this position?
    public virtual List<string> ValidateTile(Island map)
    {
        if (_tile.Building == null)
            return new List<string>();

        return new List<string>() { "There's already a building there!" };
    }

    public virtual List<string> ValidateSupport(Island map)
    {
        if (_tile.Y == 0)
            return new List<string>();

        var buildingBelow = map.Down(_tile.X, _tile.Y).Building;
        if (buildingBelow is not null && buildingBelow.BuildingType.HasRoof)
            return new List<string>();

        return new List<string>() { "Tile is not supported!" };
    }

    //Should this building render a roof?
    public virtual bool ShouldRenderRoof(Island map, BuildingType buildingType)
    {
        return map.Up(_tile.X, _tile.Y).Building is null && _tile.Building is not null && buildingType.HasRoof;
    }

    //Can the given building be built on top?
    public virtual List<string> ValidateCanBuildOnTop(Island map)
    {
        return new List<string>();
    }

    public virtual List<string> ValidateResources(BuildingType buildingType)
    {
        if (ResourceManager.Instance.CheckResourcesAvailability(buildingType.Cost) != false)
            return new List<string>();

        return new List<string>() { $"Can't place {buildingType} in because you don't have enough resources" };
    }

    public List<string> ValidateDestroyable(Island map)
    {
        if (map.Up(_tile.X, _tile.Y).Building is null)
            return new List<string>();

        return new List<string>() { "Cannot destroy this building as there is one above it!" };
    }
}
