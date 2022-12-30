using OrcaDefense.Models;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Type")]
public class BuildingType : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private BuildingTypeEnum buildingEnum;
    [SerializeField]
    private bool hasRoof;
    [SerializeField]
    private ResourceList cost;
    [SerializeField]
    private ResourceList income;
    [SerializeField]
    private int ticksPerIncome;

    //Accessors for the above
    public GameObject Prefab { get => prefab; }
    public BuildingTypeEnum BuildingEnum { get => buildingEnum; }
    public bool HasRoof { get => hasRoof; }
    public ResourceList Cost { get => cost; }
    public ResourceList Income { get => income; }
    public int TicksPerIncome { get => ticksPerIncome; }


    public IBuildingValidator GetBuildingValidator(Tile t)
    {
        return GetValidatorByBuildingType(buildingEnum, t);
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

