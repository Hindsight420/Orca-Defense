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

    //Should this building render a roof?
    public virtual bool ShouldRenderRoof(Island map)
    {
        return map.Up(_tile.X, _tile.Y).Building is null;
    }

    //Does this building care if we build it next to it's neighbours?
    public virtual string[] ValidateAdjacencies(Island map)
    {
        return new string[] { };
    }

    //Does this building care if we build X building next to it?
    public virtual string[] ValidateAdjacency(BuildingTypeEnum buildingToBuild)
    {
        return new string[] { };
    }

    //Can the building be placed in this position?
    public virtual string[] ValidatePosition(Island map)
    {
        //IsOccupied
        var errors = new List<string>();
        if (_tile.Building != null) { errors.Add("There's already a building there!"); }

        //IsSupported
        if (_tile.Y != 0 && map.Down(_tile.X, _tile.Y).Building is null) { errors.Add("Tile is not supported!"); }

        return errors.ToArray();
    }

    //Can the given building be built on top?
    public virtual string[] ValidateCanBuildOnTop(Island map)
    {
        return new string[] { };
    }

    public virtual List<string> ValidateResources(BuildingType buildingType)
    {
        if (ResourceManager.Instance.CheckResourcesAvailability(buildingType.Cost) == false)
        {
            return new List<string>() { $"Can't place {buildingType} in because you don't have enough resources" };
        }

        return new List<string>();
    }

    public List<string> ValidateBuildingPosition(Island map, BuildingTypeEnum buildingToBuild)
    {
        var errors = new List<string>();
        errors.AddRange(ValidateAdjacencies(map));
        errors.AddRange(ValidateAdjacency(buildingToBuild));
        errors.AddRange(ValidatePosition(map));

        return errors;
    }

    public List<string> ValidateDestroyable(Island map)
    {
        if(map.Up(_tile.X, _tile.Y).Building is not null)
        {
            return new List<string>() { "Cannot destroy this building as there is one above it!" };
        }
        return new List<string>();
    }
}
