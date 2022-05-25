using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    public GameObject Prefab;

    public List<ResourceValue> Cost;

    public bool hasRoof;

    public BuildingTypeEnum BuildingEnum;

    public List<ResourceValue> Income;
    public int? TicksPerIncome;

    public IBuildingValidator GetBuildingValidator(Tile t)
    {
        return GetValidatorByBuildingType(BuildingEnum, t);
    }

    public override string ToString()
    {
        return $"Building Type: {name}";
    }

    private IBuildingValidator GetValidatorByBuildingType(BuildingTypeEnum type, Tile t)
    {
        return type switch
        {
            BuildingTypeEnum.FishingHut => new FishingHutValidator(t),
            _ => new BaseBuildingValidator(t),
        };
    }
}

[Serializable]
public enum BuildingTypeEnum
{
    FishingHut,
    Default,
}

