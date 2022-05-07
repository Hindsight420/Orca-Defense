using OrcaDefense.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    public BuildingType()
    {
        BuildingValidator = GetValidatorByBuildingType(BuildingEnum, null);
    }

    public BuildingType Construct()
    {
        return new BuildingType();
    }

    public GameObject Prefab;

    public List<ResourceValue> Cost;

    public bool hasRoof;

    private BuildingTypeEnum BuildingTypeEnum;

    public BuildingTypeEnum BuildingEnum { get => BuildingTypeEnum; }

    public IBuildingValidator BuildingValidator { get; }

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
    Terrain,
    CuckShed
}

