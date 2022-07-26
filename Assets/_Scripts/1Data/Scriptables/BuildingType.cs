using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    readonly public GameObject Prefab;
    readonly public BuildingTypeEnum BuildingEnum;
    readonly public bool HasRoof;
    readonly public List<ResourceValue> Cost;

    readonly public List<ResourceValue> Income;
    readonly public int? TicksPerIncome;

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

