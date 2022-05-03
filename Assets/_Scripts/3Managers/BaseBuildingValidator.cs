using OrcaDefense.Models;
using System;

public class BaseBuildingValidator : IBuildingValidator
{
    public BaseBuildingValidator()
    {
    }

    public virtual bool ShouldRenderRoof(Island map, int x, int y)
    {
        return !map.GetTileAt(x, y + 1).IsOccupied;
    }

    public virtual string[] ValidateAdjacencies(Island map, int x, int y)
    {
        return null;
    }

    public virtual string[] ValidateAdjacency(BuildingTypeEnum buildingToBuild)
    {
        return null;
    }

    public virtual string[] ValidateCanBuildOnTop(Island map, int x, int y)
    {
        return null;
    }
}
