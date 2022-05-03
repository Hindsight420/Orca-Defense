using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    public BuildingType()
    {
        BuildingValidator = GetValidatorByBuildingType(BuildingEnum);
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

    private IBuildingValidator GetValidatorByBuildingType(BuildingTypeEnum type)
    {
        return type switch
        {
            BuildingTypeEnum.FishingHut => new FishingHutValidator(),
            _ => new BaseBuildingValidator(),
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

